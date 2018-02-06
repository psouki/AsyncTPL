using System;
using System.Threading;

namespace ThreadSync
{
    class Program
    {
        //Shared resource. Every thread will race to use this resource
        static int _sum;

        // a reference type object to be locked 
        static object _lock = new object();

        static void Main(string[] args)
        {
            Thread[] threads = new Thread[10];

            for (int i = 0; i < threads.Length; i++)
            {
                // Initialize thread with address of DoWork
                // this creates the race condition
                threads[i] = new Thread(DoWork);

                //Initialize thread with address of DoWork2 with lock
                // with this the result will the expected one 200,000
                //threads[i] = new Thread(DoWork2);
                threads[i].Start();
            }

            foreach (Thread t in threads)
            {
                // Making sure that all threads are finalized
                t.Join();
            }

            // it should result in 200.000, by executing 10 times the DoWork
            // but in fact the result is unpredictable, because it changes every time
            // due to the race condition
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] sum = {_sum}");
            Console.ReadKey();
        }

        // critical section for the race condition
        static void DoWork()
        {
            for (int i = 0; i < 20000; i++)
            {
                _sum++;
            }
        }

        // Lock with no race condition
        static void DoWork2()
        {
            //Lock to the thread to take ownership of the resource
            lock (_lock)
            {
                for (int i = 0; i < 20000; i++)
                {
                    _sum++;
                }
            }
        }
    }
}
