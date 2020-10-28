using ConferencePlanner.FrontEnd.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConferencePlanner.FrontEnd.Middleware
{
    public class RequireLoginMiddleware
    {
        private readonly RequestDelegate next;
        private readonly LinkGenerator linkGenerator;

        public RequireLoginMiddleware(RequestDelegate next, LinkGenerator linkGenerator)
        {
            this.next = next;
            this.linkGenerator = linkGenerator;
        }

        public Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (context.User.Identity.IsAuthenticated &&
                endpoint?.Metadata.GetMetadata<SkipWelcomeAttribute>() == null)
            {
                var isAttendee = context.User.IsAttendee();
                if (!isAttendee)
                {
                    var url = linkGenerator.GetUriByPage(context, page: "/Welcome");
                    context.Response.Redirect(url);

                    return Task.CompletedTask;
                }
            }

            return next(context);
        }
    }
}
