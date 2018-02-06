using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace ThreadStudy
{
    class BackgroundTest
    {
        readonly int _maxIterations;

        public BackgroundTest(int maxIterations)
        {
            _maxIterations = maxIterations;
        }

        public void RunLoop()
        {
            for (int i = 0; i < _maxIterations; i++)
            {
                var threadPosition = Thread.CurrentThread.IsBackground ? "Background Thread" : "Foreground Thread";
                Console.WriteLine($"{threadPosition} count: {i}");
                Thread.Sleep(500);
            }
            var threadPositionAtFinish = Thread.CurrentThread.IsBackground ? "Background Thread" : "Foreground Thread";
            Console.WriteLine($"{threadPositionAtFinish} finished counting.");
        }
    }
}