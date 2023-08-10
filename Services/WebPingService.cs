using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WebPulse2023.Data;
using WebPulse2023.Models;
using WebPulse2023.Hubs;
using System.Net;

namespace WebPulse2023.Services
{
    public interface IWebPingService
    {
        Task PingWebsiteAsync();
        Task RollUpStatisticsAsync(RollupInterval interval);
    }

    public class WebPingService : IWebPingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public WebPingService(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        public async Task PingWebsiteAsync()
        {
            var activeWebsites = _context.Website.Where(w => w.Active).ToList();
            var processedUrls = new HashSet<string>(); // To keep track of processed URLs

            foreach (var website in activeWebsites)
            {
                if (!processedUrls.Contains(website.Url))
                {
                    processedUrls.Add(website.Url); // Mark the URL as processed

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    using (var httpClient = _clientFactory.CreateClient())
                    {
                        HttpResponseMessage pingResult = null;

                        try
                        {
                            pingResult = await httpClient.GetAsync(website.Url);
                        }
                        catch (HttpRequestException ex)
                        {
                            pingResult = new HttpResponseMessage();
                            pingResult.StatusCode = HttpStatusCode.ServiceUnavailable; // Custom status code to indicate an error
                        }
                        finally
                        {
                            stopwatch.Stop();
                        }

                        using (var scope = _context.Database.BeginTransaction())
                        {
                            foreach (var site in _context.Website.Where(w => w.Url == website.Url))
                            {
                                var webPing = new WebPing
                                {
                                    Website = site,
                                    Timestamp = DateTime.UtcNow,
                                    isUp = pingResult.IsSuccessStatusCode,
                                    ResponseTime = pingResult.IsSuccessStatusCode ? stopwatch.ElapsedMilliseconds.ToString() : "N/A",
                                    StatusCode = (int)pingResult.StatusCode
                                };

                                _context.WebPing.Add(webPing);
                            }
                            await _context.SaveChangesAsync();
                            scope.Commit();
                        }
                        Debug.WriteLine($"Processed {website.Url}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Skipping {website.Url} because it was already processed");
                }
            }
        }

        public async Task RollUpStatisticsAsync(RollupInterval interval)
        {
            var websites = _context.Website.ToList();

            foreach (var website in websites)
            {
                var now = DateTime.UtcNow;
                DateTime timestamp;

                switch (interval)
                {
                    case RollupInterval.Hourly:
                        timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                        break;
                    case RollupInterval.Daily:
                        timestamp = new DateTime(now.Year, now.Month, now.Day);
                        break;
                    case RollupInterval.Weekly:
                        var startOfWeek = now.AddDays(-(int)now.DayOfWeek - 6); // Adjust to the week before
                        timestamp = new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day);
                        break;
                    case RollupInterval.Monthly:
                        timestamp = new DateTime(now.Year, now.Month, 1);
                        break;
                    default:
                        throw new ArgumentException("Invalid interval");
                }

                RollUpForInterval(website, interval, timestamp);
            }

            await _context.SaveChangesAsync();
        }
        private void RollUpForInterval(Website website, RollupInterval interval, DateTime timestamp)
        {
            if (interval == RollupInterval.Hourly)
            {
                var lowerLevelData = _context.WebPing
                    .Where(p => p.Website == website && p.Timestamp >= timestamp && p.Timestamp < DateTime.UtcNow.AddHours(1))
                    .ToList();

                var upCount = lowerLevelData.Count(p => p.isUp == true);
                var downCount = lowerLevelData.Count(p => p.isUp == false);

                var rollup = _context.PingStatistic.FirstOrDefault(r => r.Website == website && r.Interval == interval && r.Timestamp == timestamp);

                if (rollup == null)
                {
                    rollup = new PingStatistic
                    {
                        Website = website,
                        Interval = interval,
                        Timestamp = timestamp,
                        UpCount = upCount,
                        DownCount = downCount
                    };
                    _context.PingStatistic.Add(rollup);
                }
                else
                {
                    rollup.UpCount = upCount;
                    rollup.DownCount = downCount;
                }

                // Clean up data that's outside of the current interval
                var dataToDelete = _context.WebPing
                    .Where(p => p.Website == website && p.Timestamp < timestamp)
                    .ToList();

                _context.WebPing.RemoveRange(dataToDelete);
            }
            else if(Enum.IsDefined(typeof(RollupInterval),interval) && interval != RollupInterval.Hourly)
            {
                var lowerLevelData = _context.PingStatistic.Where(p => p.Interval == GetLowerInterval(interval) && p.Website == website);


                var rollup = _context.PingStatistic.FirstOrDefault(r => r.Website == website && r.Interval == interval && r.Timestamp == timestamp);

                if(rollup == null)
                {
                    rollup = new PingStatistic
                    {
                        Website = website,
                        Interval = interval,
                        Timestamp = timestamp,
                        UpCount = lowerLevelData.Sum(p => p.UpCount),
                        DownCount = lowerLevelData.Sum(p => p.DownCount)
                    };

                     _context.PingStatistic.Add(rollup);
                } else
                {
                    rollup.UpCount = lowerLevelData.Sum(p => p.UpCount);
                    rollup.DownCount = lowerLevelData.Sum(p => p.DownCount);
                }

                var dataToDelete = _context.PingStatistic.Where(p => p.Website == website && p.Interval == GetLowerInterval(interval) && p.Timestamp < timestamp);
                _context.PingStatistic.RemoveRange(dataToDelete);

            } else
            {
                throw new ArgumentException("Failed to rollup");
            }
        }



        private RollupInterval GetLowerInterval(RollupInterval interval)
        {
            switch(interval)
            {
                case RollupInterval.Monthly:
                    return RollupInterval.Weekly;
                case RollupInterval.Weekly:
                    return RollupInterval.Daily;
                case RollupInterval.Daily:
                    return RollupInterval.Hourly;
                default:
                    throw new ArgumentException("Unsupported interval");
            }

        }


    }
}
