using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStudy
{
    class CheatClass
    {
        #region Thread and Tasks
        #region Listing 1-1 = First example
        //public static void ThreadMethod()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Console.WriteLine("ThreadProc: {0}", i);
        //        //Thread.Sleep(1000);
        //        Thread.Sleep(0);
        //    }
        //}

        //public static void Main(string[] args)
        //{
        //    Thread t = new Thread(new ThreadStart(ThreadMethod));
        //    t.Start();

        //    for (int i = 0; i < 4; i++)
        //    {
        //        Console.WriteLine("Main thread: Do some work!");
        //        //Thread.Sleep(0);
        //        Thread.Sleep(1000);
        //    }
        //    t.Join();

        //    Console.ReadLine();
        //}
        #endregion

        #region Listing 1-2 = Background example
        //public static void ThreadMethod()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Console.WriteLine("ThreadProc: {0}", i);
        //        Thread.Sleep(1000);

        //    }
        //}
        //public static void Main(string[] args)
        //{
        //    Thread t = new Thread(new ThreadStart(ThreadMethod));
        //    t.IsBackground = true;
        //    //t.IsBackground = false;
        //    t.Start();
        //}        

        //static void Main()
        //{
        //    BackgroundTest shortTest = new BackgroundTest(10);
        //    Thread foregroundThread = new Thread(new ThreadStart(shortTest.RunLoop));

        //    BackgroundTest longTest = new BackgroundTest(10);
        //    Thread backgroundThread = new Thread(new ThreadStart(longTest.RunLoop));
        //    backgroundThread.IsBackground = true;

        //    foregroundThread.Start();
        //    backgroundThread.Start();

        //    Console.ReadLine();
        //}

        #endregion

        #region Listing 1-3 = Parameterized Thread
        //public static void ThreadMethod(object obj)
        //{
        //    for (int i = 0; i < (int)obj; i++)
        //    {
        //        Console.WriteLine("ThreadProc: {0}", i);
        //        Thread.Sleep(0);

        //    }
        //}
        //public static void Main(string[] args)
        //{
        //    Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));
        //    t.Start(5);

        //    for (int i = 0; i < 4; i++)
        //    {
        //        Console.WriteLine("Main thread: Do some work!");
        //        Thread.Sleep(1000);
        //    }
        //    t.Join();

        //    Console.ReadLine();
        //}
        #endregion

        #region Listing 1-4 = Shared Variable
        //public static void Main(string[] args)
        //{
        //    bool stopped = false;
        //    Thread t = new Thread(new ThreadStart(() =>
        //    {
        //        while (!stopped)
        //        {
        //            Console.WriteLine("Running...!");
        //            Thread.Sleep(1000);
        //        }
        //    }));
        //    t.Start();

        //    Console.WriteLine("Press any key to exit!");
        //    Console.ReadKey();
        //    stopped = true;
        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-5 = ThreadStatic
        //[ThreadStatic]
        //public static int _field;
        //public static void Main()
        //{
        //    new Thread(() =>
        //    {
        //        for (int z = 0; z < 10; z++)
        //        {
        //            _field++;
        //            Console.WriteLine("Thread A: {0}", z);
        //        }
        //    }).Start();

        //    new Thread(() =>
        //    {
        //        for (int z = 0; z < 10; z++)
        //        {
        //            _field++;
        //            Console.WriteLine("Thread B: {0}", z);
        //        }
        //    }).Start();
        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-6 = ThreadLocal
        //public static ThreadLocal<int> _field = new ThreadLocal<int>(() =>
        //{
        //    return Thread.CurrentThread.ManagedThreadId;
        //});

        //public static void Main()
        //{
        //    new Thread(() =>
        //    {
        //        for (int z = 0; z < _field.Value; z++)
        //        {

        //            Console.WriteLine($"Thread A: {z} - id {_field} ");
        //        }
        //    }).Start();

        //    new Thread(() =>
        //    {
        //        for (int z = 0; z < _field.Value; z++)
        //        {

        //            Console.WriteLine($"Thread B: {z} - id {_field} ");
        //        }
        //    }).Start();
        //    Console.ReadKey();
        //}

        #endregion

        #region Listing 1-7 = Thread Pools

        //public static void Main()
        //{
        //    ThreadPool.QueueUserWorkItem((s) =>
        //    {
        //        Console.WriteLine("Working on a thread from threadpool!");
        //    });
        //    Console.ReadLine();
        //}
        #endregion

        #region Listing 1-8 = Tasks

        //public static void Main()
        //{
        //    Task t = Task.Run(() =>
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            Console.Write("*");
        //        }
        //    });
        //    t.Wait();
        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-9 = Tasks that returns a value
        //public static void Main()
        //{
        //    Task<int> t = Task.Run(() =>
        //    {
        //        Console.Write("Enter a number: ");
        //        string s = Console.ReadLine();

        //        return int.Parse(s);
        //    });
        //    Console.WriteLine("Number typed: {0}", t.Result);
        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-10 = Tasks with continuation
        //public static void Main()
        //{
        //    Task<int> t = Task.Run(() =>
        //    {
        //        Console.Write("Enter a number: ");
        //        string s = Console.ReadLine();

        //        return int.Parse(s);
        //    }).ContinueWith((i) =>
        //    {
        //        return i.Result * 2;
        //    });
        //    Console.WriteLine("Number typed: {0}", t.Result);
        //    Console.ReadKey();
        //}

        #endregion

        #region Listing 1-11 = Tasks with continuation (overloads)

        //public static void Main()
        //{
        //    Task<int> t = Task.Run(() =>
        //    {
        //        Console.Write("Enter a number: ");
        //        string s = Console.ReadLine();

        //        return int.Parse(s);
        //    });
        //    t.ContinueWith((i) =>
        //    {
        //        Console.WriteLine("Canceled");
        //    }, TaskContinuationOptions.OnlyOnCanceled);
        //    t.ContinueWith((i) =>
        //    {
        //        Console.WriteLine("Faulted");
        //    }, TaskContinuationOptions.OnlyOnFaulted);
        //    var completedTask = t.ContinueWith((i) =>
        //    {
        //        Console.WriteLine("Completed");
        //    }, TaskContinuationOptions.OnlyOnRanToCompletion);
        //    completedTask.Wait();

        //    Console.WriteLine("Number typed: {0}", t.Result);
        //    Console.ReadKey();
        //}

        #endregion

        #region Listing 1-12 Child tasks
        //public static void Main()
        //{
        //    Task<int[]> parent = Task.Run(() =>
        //    {
        //        var results = new int[3];
        //        new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
        //        new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
        //        new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();
        //        return results;
        //    });

        //    Task finalTask = parent.ContinueWith(
        //    parentTask =>
        //    {
        //        foreach (int i in parentTask.Result)
        //            Console.WriteLine(i);
        //    });
        //    finalTask.Wait();
        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-13 TaskFactory
        //public static void Main()
        //{
        //    Task<int[]> parent = Task.Run(() =>
        //    {
        //        int[] results = new int[3];
        //        TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);
        //        tf.StartNew(() => results[0] = 0);
        //        tf.StartNew(() => results[1] = 1);
        //        tf.StartNew(() => results[2] = 2);

        //        return results;
        //    });
        //    Task finalTask = parent.ContinueWith(parentTask =>
        //    {
        //        foreach (int item in parent.Result)
        //        {
        //            Console.WriteLine(item);
        //        }
        //    });
        //    finalTask.Wait();
        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-14 WaitAll
        //public static void Main()
        //{
        //    Task[] tasks = new Task[3];

        //    tasks[0] = Task.Run(() =>
        //    {
        //        Thread.Sleep(5000);
        //        Console.WriteLine(1);
        //        return 1;
        //    });
        //    tasks[1] = Task.Run(() =>
        //    {
        //        Thread.Sleep(3000);
        //        Console.WriteLine(2);
        //        return 2;
        //    });
        //    tasks[2] = Task.Run(() =>
        //    {
        //        Thread.Sleep(1000);
        //        Console.WriteLine(3);
        //        return 3;
        //    });
        //    Task.WaitAll(tasks);

        //    Console.ReadKey();
        //}
        #endregion

        #region Listing 1-15 WaitAny
        //public static void Main()
        //{
        //    Task<int>[] tasks = new Task<int>[3];

        //    tasks[0] = Task.Run(() =>
        //    {
        //        Thread.Sleep(5000);
        //        return 1;
        //    });
        //    tasks[1] = Task.Run(() =>
        //    {
        //        Thread.Sleep(2000);
        //        return 2;
        //    });
        //    tasks[2] = Task.Run(() =>
        //    {
        //        Thread.Sleep(3000);
        //        return 3;
        //    });
        //    while (tasks.Length > 0)
        //    {
        //        int i = Task.WaitAny(tasks);
        //        Task<int> completedTask = tasks[i];
        //        Console.WriteLine(completedTask.Result);
        //        var temp = tasks.ToList();
        //        temp.RemoveAt(i);
        //        tasks = temp.ToArray();
        //    }
        //    Console.ReadKey();
        //}
        #endregion
        #endregion

        #region Parallel Class
        #region Listing 1-16 PArallel.For and Parallel.ForEach
        //public static void Main()
        //{
        //Console.WriteLine("Using C# For Loop \n");

        //for (int i = 0; i <= 10; i++)
        //{
        //    Console.WriteLine($"i = {i}, thread = {Thread.CurrentThread.ManagedThreadId}");
        //    Thread.Sleep(1000);
        //}

        //Console.WriteLine("\nUsing Parallel.For \n");

        //Parallel.For(0, 10, i =>
        //{
        //    Console.WriteLine($"i = {i}, thread = {Thread.CurrentThread.ManagedThreadId}");
        //    Thread.Sleep(1000);
        //});

        //Console.WriteLine("\nUsing Parallel.ForEach \n");

        //var numbers = Enumerable.Range(0, 10);
        //Parallel.ForEach(numbers, i =>
        //{
        //    Console.WriteLine($"i = {i}, thread = {Thread.CurrentThread.ManagedThreadId}");
        //    Thread.Sleep(1000);
        //});
        //Console.ReadKey();
        //}
        #endregion

        #region Listing 1-17 Parallel.Break
        //public static void Main()
        //{
        //    ParallelLoopResult result = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) =>
        //    {
        //        if (i == 500)
        //        {
        //            Console.WriteLine("Breaking loop");
        //            loopState.Stop();
        //        }
        //        return;
        //    });

        //    Console.WriteLine(result.IsCompleted.ToString());
        //    Console.WriteLine(result.LowestBreakIteration.ToString());
        //    Console.ReadKey();
        //}
        #endregion
        #endregion

        #region Async and Await

        #region Listing 1-18 Async/Await
        //public static void Main()
        //{
        //    string result = DownloadContent().Result;
        //    Console.WriteLine(result);
        //    Console.ReadKey();
        //}
        //public static async Task<string> DownloadContent()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string result = await client.GetStringAsync("http://www.microsoft.com");
        //        return result;
        //    }
        //}
        #endregion

        #region Listing 1-19 Scalability versus Responsiveness
        //public static void Main()
        //{

        //}
        public Task SleepAsyncA(int millisecondsTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondsTimeout));
        }

        public Task SleepAsyncB(int millisecondsTimeout)
        {
            TaskCompletionSource<bool> tcs = null;
            var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondsTimeout, -1);
            return tcs.Task;
        }
        #endregion

        #endregion
    }
}
