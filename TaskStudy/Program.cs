using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            //TaskFirstExample();
            //TaskContinuation(3,5);
            //ChildTaks();
            //WaitAllTasks();
            //WaitAnyTasks();
            AsyncAwait();
        }

        private static void TaskFirstExample()
        {
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Who Dat Nation !!");
                    Thread.Sleep(1000);
                    //Thread.Sleep(1);
                }
            });

            Console.WriteLine("Waiting the task to complete");
            // it asks the main thread to wait till the task completes
            t.Wait();
            Console.WriteLine("Main thread finishes");

            // Console.ReadKey();
        }

        private static void TaskContinuation(int a, int b)
        {
            Task<double> exp = Task.Run(() => Math.Pow(a, b));
            // Is dangerous to use t.result direct, because it blocks the main thread
            // till the task is completed.
            // a safer way to do it is to use the continue with, that means
            // after the task is completed do this.
            exp.ContinueWith(t =>
            {
                Console.WriteLine($"The result of {a} ^ {b} is {t.Result}");
            });
            Console.ReadKey();
        }

        private static void ChildTaks()
        {
            Task<List<int>> parent = Task.Run(() =>
            {
                ICollection<int> result = new List<int>();
                new Task(() => result.Add(Thread.CurrentThread.ManagedThreadId), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => result.Add(Thread.CurrentThread.ManagedThreadId), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => result.Add(Thread.CurrentThread.ManagedThreadId), TaskCreationOptions.AttachedToParent).Start();
                return result.ToList();
            });

            Task finalTask = parent.ContinueWith(t =>
            {
                foreach (int i in t.Result)
                {
                    Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] - {i}");
                    Thread.Sleep(1000);
                }
            });

            finalTask.Wait();
            Console.WriteLine("Main thread finishes");
        }

        private static void WaitAllTasks()
        {
            // all task started at the same time
            // the order is controlled by the Thread.Sleep
            Task t1 = Task.Run(() =>
            {
                Thread.Sleep(5100);
                Console.WriteLine("Alla Madrid !");
            });

            Task t2 = Task.Run(() =>
            {
                Thread.Sleep(3100);
                Console.WriteLine("Who Dat !");
            });

            Task t3 = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Aqui é Galo !");
            });

            Task.WaitAll(t1, t2, t3);

            Thread.Sleep(1000);
            Console.WriteLine("Main thread finishes");
        }

        private static void WaitAnyTasks()
        {
            ICollection<Task<string>> tasks = new List<Task<string>>();

            tasks.Add(Task.Run(() =>
            {
                Thread.Sleep(5100);
                return "Alla Madrid !";
            }));

            tasks.Add(Task.Run(() =>
            {
                Thread.Sleep(3100);
                return "Who Dat !";
            }));

            tasks.Add(Task.Run(() =>
            {
                Thread.Sleep(100);
                return "Aqui é Galo !";
            }));


            while (tasks.Any())
            {
                // wait to return the index of the completed first task 
                int test = Task.WaitAny(tasks.ToArray());
                Task<string> x = tasks.ElementAt(test);
                Console.WriteLine(x.Result);
                tasks.Remove(x);
            }

            Thread.Sleep(3000);
            Console.WriteLine("Main thread finishes");
        }

        // Example of parallel and asynchronous calls
        public static void AsyncAwait()
        {
            Uri newsLink = new Uri("http://www.neworleanssaints.com/cda-web/rss-module.htm?tagName=News");
            Uri photoLink = new Uri("http://www.neworleanssaints.com/cda-web/rss-module.htm?tagName=Photos");
            Uri videoLink = new Uri("http://www.neworleanssaints.com/cda-web/rss-module.htm?tagName=Videos");

            // List of task executing asynchronously 
            ICollection<Task<string>> tasks = new List<Task<string>>();

            Task<string> t1 = DownloadNews(newsLink);
            Task<string> t2 = DownloadNews(photoLink);
            Task<string> t3 = DownloadNews(videoLink);

            tasks.Add(t1);
            tasks.Add(t2);
            tasks.Add(t3);

            List<News> feeds = new List<News>();

            //till there is any task to be completed it continues the loop
            while (tasks.Any())
            {
                // returns the index of the first completed task 
                // no matter the order
                int index = Task.WaitAny(tasks.ToArray());
                Task<string> t = tasks.ElementAt(index);

                //while the news are being downloaded the main thread 
                // is converting the feeds into news objects
                XDocument doc = XDocument.Parse(t.Result);
                IEnumerable<News> feedsList = XmlHelper.ConvertXmlToNews(doc);
                feeds.AddRange(feedsList);
                tasks.Remove(t);
            }


            foreach (News feed in feeds)
            {
                Console.WriteLine($"Category: {feed.Category}");
                Console.WriteLine($"Title: {feed.Title}");
                Console.WriteLine($"Description: {feed.Description}");
                Console.WriteLine($"Published date: {feed.PublishDate}");
                Console.WriteLine(Environment.NewLine);
            }
            Console.ReadKey();
        }


        // As the context and the calling doesn't run in the same thread in the console application
        // the task.waitAny, task.waitAll, task.Wait or Task.Result will not create a deadlock
        // but when called in a GUI or ASP.NET context it will in the calling method,
        // like the WaitAnyTasks method above
        public static async Task<string> DownloadNews(Uri newsLink)
        {
            await Task.Delay(3000);
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync(newsLink);
                return result;
            }
        }

    }
}
