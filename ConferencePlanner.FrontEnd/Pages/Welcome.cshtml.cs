using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConferencePlanner.FrontEnd.Infrastructure;
using ConferencePlanner.FrontEnd.Pages.Models;
using ConferencePlanner.FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConferencePlanner.FrontEnd.Pages
{
    [SkipWelcome]
    public class WelcomeModel : PageModel
    {
        private readonly IConferenceApiClient apiClient;

        public WelcomeModel(IConferenceApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }

        public IActionResult OnGet()
        {
            // Redirect to home page if user is anonymous or already registered as attendee
            var isAttendee = User.IsAttendee();

            if (!User.Identity.IsAuthenticated || isAttendee)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await apiClient.AddAttendeeAsync(Attendee);

            if (!success)
            {
                ModelState.AddModelError("", "There was an issue creating the attendee for this user.");
                return Page();
            }

            // Re-issue the auth cookie with the new IsAttendee claim
            User.MakeAttendee();
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, User);

            return RedirectToPage("/Index");
        }
    }
}
