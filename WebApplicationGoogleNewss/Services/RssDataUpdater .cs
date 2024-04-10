using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplicationGoogleNewss.Services
{
    public class RssDataUpdater
    {
        private readonly string _rssFeedUrl;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _refreshInterval;  // The scheduled refresh time

        public RssDataUpdater(IConfiguration configuration, IMemoryCache cache)
        {
            _rssFeedUrl = configuration["AppSettings:RssFeedUrl"];  // Setting the RSS feed address from the settings
            _httpClient = new HttpClient();
            _cache = cache;
            _refreshInterval = TimeSpan.FromMinutes(5); // Setting the refresh time to 5 minutes
        }

        // A function that starts the process of updating the RSS data asynchronously according to the refresh time
        public async Task StartUpdatingRssDataAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RefreshRssDataAsync();  // Update the RSS data
                await Task.Delay(_refreshInterval, cancellationToken);  // Wait until the refresh timeout
            }
        }

        // A function that updates the RSS data and stores it in the cache
        private async Task RefreshRssDataAsync()
        {
            string rssData = await _httpClient.GetStringAsync(_rssFeedUrl); // Getting the RSS data from the URL
            _cache.Set("RssData", rssData, _refreshInterval); // Saving the data in the cache for the duration of the refresh
        }
    }
}
