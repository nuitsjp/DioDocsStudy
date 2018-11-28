using System;
using BenchmarkDotNet.Running;
using Benchmarks;

namespace BenchmarksForExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<Benchmarks>();
            var summary = BenchmarkRunner.Run<Benchmark>();
            Console.ReadKey();
        }
    }
}
