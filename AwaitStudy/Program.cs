using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AwaitStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            //ICollection<News> result = await GetNews();
            //foreach (News feed in result)
            //{
            //    Console.WriteLine($"Category: {feed.Category}");
            //    Console.WriteLine($"Title: {feed.Title}");
            //    Console.WriteLine($"Description: {feed.Description}");
            //    Console.WriteLine($"Published date: {feed.PublishDate}");
            //    Console.WriteLine(Environment.NewLine);
            //}
            //SynchronizationContext.Current.Send(x=>Deadlock(), null);
            Console.WriteLine("Main Start");
            DelayAsync();
            Console.WriteLine("Main End");
            Console.ReadKey();
        }

        private static async Task DelayAsync()
        {
            await Task.Delay(1000);
            Console.WriteLine("DelayAsync.Start");
            Console.WriteLine("DelayAsync.End");
        }

        // This method causes a deadlock when called in a GUI or ASP.NET context.
        public static void Deadlock()
        {
            Console.WriteLine("Deadlock.Start");
            // Start the delay.
            var delayTask = DelayAsync();
            // Wait for the delay to complete.
            delayTask.Wait();
            Console.WriteLine("Deadlock.End");
        }

        //static async Task WaitAsync()
        //{
        //    await Task.Delay(2000);
        //}

        //static async Task<string> testeTask()
        //{
        //    await Task.Delay(2000);
        //    return "Dead lock";
        //} 
        //static void Deadlock()
        //{
        //    Task task = WaitAsync();

        //    task.Wait();

        //    //var test = testeTask().Result;
        //}
        static async Task<List<News>> GetNews()
        {
            string xmlNews;
            using (HttpClient client = new HttpClient())
            {
                Uri newsLink = new Uri("http://www.neworleanssaints.com/cda-web/rss-module.htm?tagName=News");
                xmlNews = await DownloadNews(newsLink);
            }

            List<News> result = await ConvertXmlToNews(xmlNews);
            return result;
        }

        public static async Task<string> DownloadNews(Uri newsLink)
        {
            await Task.Delay(3000);
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync(newsLink);
                return result;
            }
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
