using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.ConferenceDTO;
using ConferencePlanner.FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ConferencePlanner.FrontEnd.Pages
{
    [Authorize]
    public class MyAgendaModel : IndexModel
    {
        public MyAgendaModel(
            ILogger<MyAgendaModel> logger,
            IConferenceApiClient apiClient) : base(logger, apiClient)
        {

        }
        protected override Task<List<SessionResponse>> GetSessionsAsync()
        {
            return conferenceApiClient.GetSessionsByAttendeeAsync(User.Identity.Name);
        }
    }
}
