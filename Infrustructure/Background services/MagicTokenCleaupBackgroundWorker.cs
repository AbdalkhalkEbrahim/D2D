using Infrastructure.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Background_services
{
    public class MagicTokenCleaupBackgroundWorker:BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MagicTokenCleaupBackgroundWorker(IServiceProvider serviceProvider)
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

                    var expiredOrUsedMagicTokens = context.MagicTokens.Where(x => x.Expiration <= DateTime.UtcNow || x.IsUsed);
                    if (expiredOrUsedMagicTokens.Any())
                    {
                        context.MagicTokens.RemoveRange(expiredOrUsedMagicTokens);
                        await context.SaveChangesAsync(stoppingToken);
                    }

                }
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
