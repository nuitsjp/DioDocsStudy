using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Benchmarks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MultiThreadBenchmarksForExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("|Name|Thread count|Exec count|Elapsed|");
            Console.WriteLine("|--|--:|--:|--:|");
            var benchmark = new Benchmark();
            //var threadCounts = new [] {1, 1, 2, 4, 5, 8, 10, 12, 15, 20, 32, 40};
            var threadCounts = new[] { 1, 1, 2, 4 };
            //Run("Calculate", Calculate, threadCounts);
            Run("DioDocs", benchmark.DioDocs, threadCounts);
            //Run("ClosedXML", benchmark.ClosedXML, threadCounts);
            ////Run("OpenXml", benchmark.OpenXml, threadCounts);
            //Run("EPPlus", benchmark.EPPlus, threadCounts);
            //Run("NPOI", benchmark.NPOI, threadCounts);

            Console.WriteLine("Completed.");
            Console.ReadKey();
        }

        private static void Run(string name, Action action, params int[] threadCounts)
        {
            foreach (var threadCount in threadCounts)
            {
                new BenchmarkRunner(threadCount, action, name).Run();
            }
        }
    }
}
