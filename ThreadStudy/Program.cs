using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ThreadStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Main Thread called !");

            //FirstThreadExample();
            //BackgroundEx();
            //ParameterizedThread();

            //Thread t= new Thread(SayHello);
            //t.IsBackground = true;
            //t.Start(10);

            // This keeps the main thread alive
            //Console.ReadKey();

            // to keep the thread just for more 3 seconds use
            // Thread.Sleep(3000);

            ThreadPoolExample();

            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Main thread finish !");

            //Thread Local
            //new Thread(() =>
            //{
            //    for (int i = 0; i < _field.Value; i++)
            //    {
            //        Console.WriteLine($"Thread A: {i} {_field}");
            //    }
            //}).Start();

            //new Thread(() =>
            //{
            //    for (int i = 0; i < _field.Value; i++)
            //    {
            //        Console.WriteLine($"Thread B: {i} {_field}");
            //    }
            //}).Start();

            Console.ReadKey();
        }

        private static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ThreadProc");
                //Thread.Sleep(0);
                //Thread.Sleep(500);
                Thread.Sleep(1000);
            }
        }

        private static void FirstThreadExample()
        {
            Thread t = new Thread(ThreadMethod);
            t.Start();

            // This the main thread, when it has a minimum delay the new thread acts
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Main thread : core count {Environment.ProcessorCount}");
                Thread.Sleep(1000);
                //Thread.Sleep(0);
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    Console.WriteLine("Main thread: Do MORE work");
            //    //Thread.Sleep(1000);
            //    Thread.Sleep(500);
            //    //Thread.Sleep(0);
            //}

            string threadStatus1 = t.IsAlive ? "I'm alive" : "I'm gone !";
            Console.WriteLine($"{threadStatus1}");

            // this is to certify that the thread is dead
            t.Join();
            string threadStatus2 = t.IsAlive ? "I'm alive" : "I'm gone !";
            Console.WriteLine($"{threadStatus2}");
        }

        private static void BackgroundEx()
        {
            // As this a foreground thread it will be alive until it finishes the loop 
            BackgroundTest shortTest = new BackgroundTest(7);
            Thread foregroundThread = new Thread(shortTest.RunLoop);

            // As this is a background thread it remains alive as long exists a foreground thread 
            BackgroundTest longTest = new BackgroundTest(21);
            Thread backgroundThread = new Thread(longTest.RunLoop) { IsBackground = true };

            foregroundThread.Start();
            backgroundThread.Start();
        }

        private static void ParameterizedThread()
        {
            Thread t = new Thread(ParameterizedThreadMethod);

            //in a parameterized thread, the argument is passed with the start method
            t.Start(7);

            BackgroundTest bckTest = new BackgroundTest(10);
            Thread t2 = new Thread(bckTest.RunLoop) { IsBackground = true };
            t2.Start();
        }

        private static void ParameterizedThreadMethod(object num)
        {
            int count;
            int.TryParse(num.ToString(), out count);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ThreadProc");
                //Thread.Sleep(500);
                // Thread.Sleep(1000);
            }
        }

        private static void SayHello(object arg)
        {
            int interations = arg == null ? 10 : (int)arg;

            for (int i = 0; i < interations; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Hey you ! -{i}- {Thread.CurrentThread.IsBackground}");
                Thread.Sleep(1000);
            }
        }

        // It is a container that holds a separate value for every thread. 
        // it is often convenient for storing per-thread counters, resources, or partial results.
        // it is better than [ThreadStatic]
        // be careful to use this with thread pools !!
        private static ThreadLocal<int> _field = new ThreadLocal<int>(() =>
            Thread.CurrentThread.ManagedThreadId
        );

        static void ThreadPoolExample()
        {
            ThreadPool.QueueUserWorkItem(SayHello);
            Console.WriteLine("Main thread does some work, then sleeps.");
            Thread.Sleep(3000);
        }


    }
}
