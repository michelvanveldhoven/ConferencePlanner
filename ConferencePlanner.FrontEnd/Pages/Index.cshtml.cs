using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.ConferenceDTO;
using ConferencePlanner.FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ConferencePlanner.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        protected readonly IConferenceApiClient conferenceApiClient;

        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }

        public IEnumerable<(int Offset, DayOfWeek? DayofWeek)> DayOffsets { get; set; }

        public int CurrentDayOffset { get; set; }

        public bool IsLoadingData { get; set; }

        public bool IsAdmin { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public List<int> UserSessions { get; set; } = new List<int>();

        public IndexModel(ILogger<IndexModel> logger, 
            IConferenceApiClient conferenceApiClient
        )
        {
            _logger = logger;
            this.conferenceApiClient = conferenceApiClient;
            Message = "WelcomeBack";
        }


        public async Task OnGetAsync(int day = 0)
        {
            Message = "WelcomeBack";
            IsAdmin = User.IsAdmin();
            IsLoadingData = true;
            CurrentDayOffset = day;

            if (User.Identity.IsAuthenticated)
            {
                var userSessions = await conferenceApiClient.GetSessionsByAttendeeAsync(User.Identity.Name);
                UserSessions = userSessions.Select(u => u.Id).ToList();
            }

            var sessions = await GetSessionsAsync();
            var startDate = sessions.Min(s => s.StartTime?.Date);
            DayOffsets = sessions.Select(s => s.StartTime?.Date)
                .Distinct()
                .OrderBy(d => d)
                .Select(day => ((int)Math.Floor((day.Value - startDate)?.TotalDays ?? 0),
                            day?.DayOfWeek))
                .ToList();

            var filterDay = startDate?.AddDays(day);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDay)
                .OrderBy(s => s.TrackId)
                .GroupBy(s => s.StartTime)
                .OrderBy(g => g.Key);

            IsLoadingData = false;
        }

        protected virtual Task<List<SessionResponse>> GetSessionsAsync()
        {
            return conferenceApiClient.GetSessionsAsync();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await conferenceApiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int sessionId)
        {
            await conferenceApiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);
            return RedirectToPage();
        }
    }
}
