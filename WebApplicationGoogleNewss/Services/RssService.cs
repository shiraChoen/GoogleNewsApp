using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplicationGoogleNewss.Services
{
    public class RssService
    {
        private readonly string _rssFeedUrl;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "RssData";

        public RssService(IConfiguration configuration, IMemoryCache cache)
        {
            _rssFeedUrl = configuration["AppSettings:RssFeedUrl"];
            _httpClient = new HttpClient();
            _cache = cache;
        }

        // Fetch RSS data from the source and update the cache
        public async Task RefreshRssDataAsync()
        {
            try
            {
                string rssData = await _httpClient.GetStringAsync(_rssFeedUrl);
                _cache.Set(CacheKey, rssData, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            }
            catch (Exception ex)
            {
                // Handle exception, such as logging it
                Console.WriteLine($"Error refreshing RSS data: {ex.Message}");
            }
        }

        // Get RSS data from cache or refresh if necessary
        public async Task<string> GetRssDataAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out string rssData))
            {
                await RefreshRssDataAsync();
            }
            return rssData;
        }
    }
}




