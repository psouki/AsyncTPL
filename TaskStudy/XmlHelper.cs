using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskStudy
{
    public class XmlHelper
    {
        public static ICollection<News> ConvertXmlToNews(XDocument doc)
        {
            XElement category = doc.Root?.Element("channel")?.Element("title");
            string categoryName = category?.Value.Split(':')[1];

            var query = doc.Descendants("item");
            return query.Select(element => new News
            {
                Title = element.Element("title")?.Value,
                Description = element.Element("description")?.Value,
                PublishDate = element.Element("pubDate")?.Value,
               Category = categoryName
            }).ToList();
        }
    }
}
