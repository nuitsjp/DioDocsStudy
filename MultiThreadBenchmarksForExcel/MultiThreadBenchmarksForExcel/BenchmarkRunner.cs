using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MultiThreadBenchmarksForExcel
{
    public class BenchmarkRunner
    {
        private static readonly int TotalExecuteCount = 960;

        public readonly int _threadCount;
        public string Name { get; }
        public int ExecuteCount => TotalExecuteCount / _threadCount;
        public Action Action { get; }

        public int WaitCount { get; set; }

        public BenchmarkRunner(int threadCount, Action action, string name)
        {
            _threadCount = threadCount;
            Action = action;
            Name = name;
        }

        public void Run()
        {
            var threads = new List<Thread>();
            for (int i = 0; i < _threadCount; i++)
            {
                var thread = new Thread(new Worker(i, this).Work);
                thread.Start();
                threads.Add(thread);
            }

            var stopwatch = new Stopwatch();
            Monitor.Enter(this);
            try
            {
                while (true)
                {
                    if (_threadCount == WaitCount) break;
                    Monitor.Wait(this, TimeSpan.FromMilliseconds(100));
                }
                stopwatch.Start();
                Monitor.PulseAll(this);
            }
            finally
            {
                Monitor.Exit(this);
            }

            threads.ForEach(x => x.Join());
            stopwatch.Stop();

            Console.WriteLine($"|{Name}|{_threadCount}|{ExecuteCount}|{stopwatch.Elapsed}|");
        }
    }


    public class Worker
    {
        private readonly int _threadNo;
        private readonly BenchmarkRunner _benchmarkRunner;
        public Worker(int threadNo, BenchmarkRunner benchmarkRunner)
        {
            _threadNo = threadNo;
            _benchmarkRunner = benchmarkRunner;
        }

        public void Work()
        {
            Monitor.Enter(_benchmarkRunner);
            try
            {
                //Console.WriteLine($"thread no:{_threadNo} Wait.");
                _benchmarkRunner.WaitCount++;
                Monitor.Wait(_benchmarkRunner);
            }
            finally
            {
                Monitor.Exit(_benchmarkRunner);
            }

            //Console.WriteLine($"thread no:{_threadNo} wake up");
            for (int i = 0; i < _benchmarkRunner.ExecuteCount; i++)
            {
                _benchmarkRunner.Action();
            }
        }
    }
}
