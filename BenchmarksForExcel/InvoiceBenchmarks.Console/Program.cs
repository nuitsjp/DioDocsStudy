using System;
using BenchmarkDotNet.Running;

namespace InvoiceBenchmarks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
            System.Console.ReadKey();
        }
    }
}
