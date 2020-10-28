using ConferencePlanner.FrontEnd.Services;
using ConferencePlannerFrontEnd.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConferencePlanner.FrontEnd.Areas.Identity.Data
{
    public class ClaimsPrincipalFactry : UserClaimsPrincipalFactory<User>
    {
        private readonly IConferenceApiClient apiClient;
        public ClaimsPrincipalFactry(
            IConferenceApiClient apiClient,
            UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor) :
            base(userManager, optionsAccessor)
        {
            this.apiClient = apiClient;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            if (user.IsAdmin)
            {
                identity.MakeAdmin();
            }

            var attendee = await apiClient.GetAttendeeAsync(user.UserName);
            if (attendee != null)
            {
                identity.MakeAttendee();
            }

            return identity;
        }
    }
}
