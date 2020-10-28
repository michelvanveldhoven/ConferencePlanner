using ConferencePlanner.FrontEnd.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConferencePlanner.FrontEnd.HealthChecks
{
    public class BackendHealthCheck : IHealthCheck
    {
        private readonly IConferenceApiClient client;

        public BackendHealthCheck(IConferenceApiClient client)
        {
            this.client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await client.CheckHealthAsync())
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}
