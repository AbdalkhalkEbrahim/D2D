using Infrastructure.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Background_services
{
    public class OtpCleanupBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OtpCleanupBackgroundWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<D2DContext>();

                    var expiredOtps = context.Otps.Where(x => x.ExpirationTime <= DateTime.UtcNow);
                    if (expiredOtps.Any())
                    {
                        context.Otps.RemoveRange(expiredOtps);
                        await context.SaveChangesAsync(stoppingToken);
                    }

                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
