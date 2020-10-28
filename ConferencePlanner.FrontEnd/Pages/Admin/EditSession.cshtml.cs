using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.ConferenceDTO;
using ConferencePlanner.FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConferencePlanner.FrontEnd.Pages.Admin
{
    public class EditSessionModel : PageModel
    {
        private readonly IConferenceApiClient apiClient;

        public EditSessionModel(IConferenceApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        [BindProperty]
        public Session Session { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGetAsync(int sessionId)
        {
            var session = await apiClient.GetSessionAsync(sessionId);
            Session = new Session
            {
                Id = session.Id,
                TrackId = session.TrackId,
                Title = session.Title,
                Abstract = session.Abstract,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session updated successfully!";
            await apiClient.PutSessionAsync(Session);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var session = await apiClient.GetSessionAsync(id);

            if (session != null)
            {
                await apiClient.DeleteSessionAsync(id);
            }
            Message = "Session deleted successfully!";
            return Page();
        }
    }
}
