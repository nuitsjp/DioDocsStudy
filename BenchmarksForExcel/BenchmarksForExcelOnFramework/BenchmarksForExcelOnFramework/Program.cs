using System;
using BenchmarkDotNet.Running;
using BenchmarksForFramework;

namespace BenchmarksForExcelOnFramework
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmark>();
            Console.ReadKey();
        }
    }
}