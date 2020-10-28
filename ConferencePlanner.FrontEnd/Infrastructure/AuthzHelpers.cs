using ConferencePlanner.FrontEnd.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthzHelpers
    {
        public static AuthorizationPolicyBuilder RequireIsAdminClaim(this AuthorizationPolicyBuilder policyBuilder) =>
            policyBuilder.RequireClaim(AuthConstants.IsAdmin, AuthConstants.TrueValue);
    }
}
