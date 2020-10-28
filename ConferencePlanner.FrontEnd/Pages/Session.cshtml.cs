using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.ConferenceDTO;
using ConferencePlanner.FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ConferencePlanner.FrontEnd.Pages
{
    public class SessionModel : PageModel
    {
        private readonly ILogger<SessionModel> logger;
        private readonly IConferenceApiClient conferenceApiClient;

        public SessionModel(
            ILogger<SessionModel> logger,
            IConferenceApiClient apiClient)
        {
            this.logger = logger;
            this.conferenceApiClient = apiClient;
        }

        public SessionResponse Session { get; set; }

        public int? DayOffSet { get; set; }

        public bool IsInPersonalAgenda { get; set; }

        public async Task<IActionResult> OnGetAsync(int sessionId)
        {
            Session = await conferenceApiClient.GetSessionAsync(sessionId);
            if (Session == null)
            {
                return RedirectToPage("/index");
            }

            var allSessions = await conferenceApiClient.GetSessionsAsync();
            var startDate = allSessions.Min(s => s.StartTime?.Date);
            DayOffSet = Session.StartTime?.Subtract(startDate ?? DateTimeOffset.MinValue).Days;

            if (User.Identity.IsAuthenticated)
            {
                var sessions = await conferenceApiClient.GetSessionsByAttendeeAsync(User.Identity.Name);
                IsInPersonalAgenda = sessions.Any(s => s.Id == sessionId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await conferenceApiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int sessionId)
        {
            await conferenceApiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);
            return RedirectToPage();
        }
    }
}
