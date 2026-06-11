using Infrastructure.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Background_services
{
    public class RefreshTokenCleanupBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RefreshTokenCleanupBackgroundWorker(IServiceProvider serviceProvider)
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

                    var endedTokens = context.RefreshTokens.Where(t=> t.ExpiresAt <= DateTime.UtcNow || t.IsRevoked);
                    if (endedTokens.Any())
                    {
                        context.RefreshTokens.RemoveRange(endedTokens);
                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        
        }
    }
}