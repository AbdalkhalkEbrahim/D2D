using Infrastructure.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Background_services
{
    public class D2DBackgroundServices : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private DateTime _lastMagicTokenCleanup = DateTime.MinValue;
        private DateTime _lastRefreshTokenCleanup = DateTime.MinValue;
        private DateTime _lastOtpLockoutEndCleanup = DateTime.MinValue;

        public D2DBackgroundServices(IServiceProvider serviceProvider)
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
                    var now = DateTime.UtcNow;

                    await context.Otps.Where(x => x.ExpirationTime <= now).ExecuteDeleteAsync(stoppingToken);

                    var targetTime = now.AddMinutes(-15);
                    await context.Users
                    .Where(u => u.OtpLockoutEnd != null && u.OtpLockoutEnd.Value <= targetTime)
                    .ExecuteUpdateAsync(setters =>
                    
                        setters.SetProperty(u => u.OtpLockoutEnd, (DateTime?)null).
                        SetProperty(u => u.OtpLockoutCount, 1)
                    
                    , stoppingToken);

                    if (now >= _lastMagicTokenCleanup.AddHours(1))
                    {
                        await context.MagicTokens.Where(x => x.Expiration <= now || x.IsUsed).ExecuteDeleteAsync(stoppingToken);
                        _lastMagicTokenCleanup = now;
                    }

                    if (now >= _lastRefreshTokenCleanup.AddDays(1))
                    {
                        await context.RefreshTokens.Where(x => x.ExpiresAt <= now || x.IsRevoked).ExecuteDeleteAsync(stoppingToken);
                        _lastRefreshTokenCleanup = now;
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}