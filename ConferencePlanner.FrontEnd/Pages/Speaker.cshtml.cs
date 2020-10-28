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
    public class SpeakerModel : PageModel
    {
        private readonly ILogger<SpeakerModel> logger;
        private IConferenceApiClient conferenceApiClient;

        public SpeakerModel(ILogger<SpeakerModel> logger,
            IConferenceApiClient apiClient)
        {
            this.logger = logger;
            this.conferenceApiClient = apiClient;
        }

        public SpeakerResponse Speaker { get; set; }

        public async Task<IActionResult> OnGetAsync(int speakerId)
        {
            Speaker = await conferenceApiClient.GetSpeakerAsync(speakerId);
            if (Speaker == null)
            {
                return NotFound();
            }

            return Page();

        }
    }
}
