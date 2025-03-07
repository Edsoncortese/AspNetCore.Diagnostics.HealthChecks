using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.SignalR
{
    public class SignalRHealthCheck : IHealthCheck
    {
        private readonly Func<HubConnection> _hubConnectionBuilder;

        public SignalRHealthCheck(Func<HubConnection> hubConnectionBuilder)
        {
            _hubConnectionBuilder = Guard.ThrowIfNull(hubConnectionBuilder);
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HubConnection? connection = null;

            try
            {
                connection = _hubConnectionBuilder();

                await connection.StartAsync(cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
            finally
            {
                if (connection != null)
                    await connection.DisposeAsync();
            }
        }
    }
}
