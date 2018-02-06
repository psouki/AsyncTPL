using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AsyncApp
{
    public class Repository
    {
        public async Task<List<News>> GetNews(string category)
        {
           await Task.Delay(3000);
            using (HttpClient client = new HttpClient())
            {
                InitializeClient(client);
                HttpResponseMessage response = await client.GetAsync(category);
                if (!response.IsSuccessStatusCode) return new List<News>();

                string result = await response.Content.ReadAsStringAsync();

                return await ConvertXmlToNews(result);
            }
        }
        
        private static void InitializeClient(HttpClient client)
        {
            client.BaseAddress = new Uri("http://www.neworleanssaints.com/cda-web/rss-module.htm?");
        }

        public static async Task<List<News>> ConvertXmlToNews(string response)
        {
            XDocument doc = XDocument.Parse(response);

            XElement category = doc.Root?.Element("channel")?.Element("title");
            string categoryName = category?.Value.Split(':')[1];

            var query = doc.Descendants("item");
            List<News> result = await Task.Run(() =>
                        query.Select(element => new News
                        {
                            Title = element.Element("title")?.Value,
                            Description = element.Element("description")?.Value,
                            PublishDate = element.Element("pubDate")?.Value,
                            Category = categoryName
                        }).Take(7).ToList());

            return result;
        }

    }
}
