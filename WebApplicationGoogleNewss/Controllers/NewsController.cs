using Microsoft.AspNetCore.Mvc;
using WebApplicationGoogleNewss.Services;
using System.Threading.Tasks;

namespace WebApplicationGoogleNewss.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : Controller
    {
        private readonly RssService _rssService;

        public NewsController(RssService rssService)
        {
            _rssService = rssService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //This action invokes the RssService service to get or refresh the RSS data from the cache, and returns as an HTTP response
            var rssData = await _rssService.GetRssDataAsync();
            return Ok(rssData);
        }
    }
}
