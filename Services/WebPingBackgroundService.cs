using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebPulse2023.Models;
using WebPulse2023.Services;

namespace WebPulse2023.Services
{
    public class WebPingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public WebPingBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var webPingService = scope.ServiceProvider.GetRequiredService<IWebPingService>();
                    await webPingService.PingWebsiteAsync();

                    var now = DateTime.UtcNow;

                    // Queue up the intervals to be triggered
                    var tasksToRun = new List<Task>();

                    // Hourly Interval: Trigger at the top of every hour
                    if (now.Minute == 0)
                    {
                        tasksToRun.Add(webPingService.RollUpStatisticsAsync(RollupInterval.Hourly));
                    }

                    // Daily Interval: Trigger at midnight
                    if (now.Hour == 0 && now.Minute == 5)
                    {
                        tasksToRun.Add(webPingService.RollUpStatisticsAsync(RollupInterval.Daily));
                    }

                    // Weekly Interval: Trigger on Sundays at midnight
                    if (now.DayOfWeek == DayOfWeek.Sunday && now.Hour == 0 && now.Minute == 10)
                    {
                        tasksToRun.Add(webPingService.RollUpStatisticsAsync(RollupInterval.Weekly));
                    }

                    // Monthly Interval: Trigger on the 1st of the month at midnight
                    if (now.Day == 1 && now.Hour == 0 && now.Minute == 15)
                    {
                        tasksToRun.Add(webPingService.RollUpStatisticsAsync(RollupInterval.Monthly));
                    }

                    foreach(var task in tasksToRun)
                    {
                        await task;
                    }
                }

                // Wait for 30 seconds before the next iteration
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
