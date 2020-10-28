using ConferencePlanner.FrontEnd.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal principal) =>
            principal.HasClaim(AuthConstants.IsAdmin, AuthConstants.TrueValue);

        public static void MakeAdmin(this ClaimsPrincipal principal) =>
            principal.Identities.First().MakeAdmin();

        public static void MakeAdmin(this ClaimsIdentity identity) =>
            identity.AddClaim(new Claim(AuthConstants.IsAdmin, AuthConstants.TrueValue));

        public static bool IsAttendee(this ClaimsPrincipal principal) =>
            principal.HasClaim(AuthConstants.IsAttendee, AuthConstants.TrueValue);

        public static void MakeAttendee(this ClaimsPrincipal principal) =>
            principal.Identities.First().MakeAttendee();

        public static void MakeAttendee(this ClaimsIdentity identity) =>
            identity.AddClaim(new Claim(AuthConstants.IsAttendee, AuthConstants.TrueValue));
    }
}
