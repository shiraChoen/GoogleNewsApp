using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using WebApplicationGoogleNewss.Models.Entity;
using WebApplicationGoogleNewss.Services;

namespace WebApplicationGoogleNewss.Services
{
    public class DalService
    {
        private readonly RssService _rssService;

        public DalService(RssService rssService)
        {
            _rssService = rssService;
        }

        // A function that returns all items from the RSS
        public async Task<IEnumerable<New>> GetAllNews()
        {
            var rssData = await _rssService.GetRssDataAsync(); //Calling the function GetRssDataAsync from RssService and returning the result

            XDocument doc = XDocument.Parse(rssData);
            var items = from item in doc.Descendants("item")
                        select new New
                        {
                            Title = item.Element("title").Value,
                            Description = item.Element("description").Value,
                            Link = item.Element("link").Value
                        };

            return items;
        }
    }
}
