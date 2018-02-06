using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace AsyncApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        readonly Repository _repository = new Repository();
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void taskBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Task<List<News>> newsTask = _repository.GetNews("?tagName=News");
            newsTask.ContinueWith(t =>
            {
                foreach (News news in t.Result)
                {
                    listBox.Items.Add(news);
                }

                sw.Stop();
                timeValueLbl.Content = $"{sw.ElapsedMilliseconds} milliseconds";
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        // it is acceptable to use async void just because it is a event handler 
        // otherwise it should return a Task or a Task<T>
        private async void AwaitBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            IEnumerable<News> news = await _repository.GetNews("?tagName=Photos");
            foreach (News item in news)
            {
                listBox.Items.Add(item);
            }
            sw.Stop();
            timeValueLbl.Content = $"{sw.ElapsedMilliseconds} milliseconds";
        }

        private void ClearListBox()
        {
            listBox.Items.Clear();
            timeValueLbl.Content = string.Empty;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
        }

        private void DeadlockBtn_Click(object sender, RoutedEventArgs e)
        {
            // this will create a deadlock because the await and the WaitAll are in the same thread.
            // the WaitAll blocked the thread till it finishes, but as it in the same thread 
            // the await doesn't finishes because the thread is blocked.
            Task result = DeadLock();
            Task.WaitAll(result);

            // This will block the UI thread till it returns a result 
            // but won't create a deadlock because it is in a different thread of the UI, 
            // but if we use the DeadLock().Result directly it will cause the deadlock.
            //var test = Task.Run(() => DeadLock()).Result;

            //if starts to use async/await the best practice is to use it all the way.
        }

        private async Task<string> DeadLock()
        {
            await Task.Delay(2000);
            return "Dead lock";
        }

        //if starts to use async/await the best practice is to use it all the way.
        private async void MultipleAsync_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // each call will wait the task to be completed 
            // so the time elapsed will be the sum of the three calls
            IEnumerable<News> photos = await _repository.GetNews("?tagName=Photos");
            IEnumerable<News> news = await _repository.GetNews("?tagName=News");
            IEnumerable<News> videos = await _repository.GetNews("?tagName=Videos");

            List<News> feeds = new List<News>();
            feeds.AddRange(photos);
            feeds.AddRange(news);
            feeds.AddRange(videos);

            foreach (News feed in feeds)
            {
                listBox.Items.Add(feed);
            }

            sw.Stop();
            timeValueLbl.Content = $"{sw.ElapsedMilliseconds} milliseconds";
        }

        private async void AsyncParallel_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //-----
            // It won't work because the parallel won't await the asynchronous task to finish

            //IEnumerable<string> source = new List<string>()
            //{ "?tagName=Photos" ,  "?tagName=News", "?tagName=Videos" };

            //List<News> result = new List<News>();
            //Parallel.ForEach(source, async i =>
            //{
            //    IEnumerable<News> list = await _repository.GetNews(i);
            //    result.AddRange(list);
            //});

            // down here the parallel will be with the status of completed and the list will have 0 count
            // unless you put Thread.Sleep to make it wait.
            //-----

            // each call will starts simultaneously 
            // so the time elapsed will be less than the sequential call
            Task<List<News>> t1 = _repository.GetNews("?tagName=Photos");
            Task<List<News>> t2 = _repository.GetNews("?tagName=News");
            Task<List<News>> t3 = _repository.GetNews("?tagName=Videos");

            ICollection<Task<List<News>>> tasks = new List<Task<List<News>>>() {t1, t2,t3};

            while (tasks.Any())
            {
                // the use of await prevents the deadlock
                Task<List<News>> list = await Task.WhenAny(tasks);

                // as soon as each task completes, the elements are added to the listbox
                foreach (News news in await list)
                {
                    listBox.Items.Add(news);
                }
                tasks.Remove(list);
            }

            // we can also wait all task to completed
            //await Task.WhenAll(tasks);

            // Then add the result to the listbox.
            //List<News> feeds = new List<News>();
            //foreach (Task<List<News>> task in tasks)
            //{
            //    feeds.AddRange(await task);
            //}

            //foreach (News feed in feeds)
            //{
            //    listBox.Items.Add(feed);
            //}

            sw.Stop();
            timeValueLbl.Content = $"{sw.ElapsedMilliseconds} milliseconds";
        }

        private async void CancelableBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();
            _cancellationTokenSource = new CancellationTokenSource();
            CancelBtn.IsEnabled = true;

            try
            {
                IEnumerable<News> news = await _repository.GetNews("?tagName=Photos", _cancellationTokenSource.Token);
                foreach (News item in news)
                {
                    listBox.Items.Add(item);
                }
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message, "Canceled !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception !");
            }
            finally
            {
                CancelBtn.IsEnabled = false;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
