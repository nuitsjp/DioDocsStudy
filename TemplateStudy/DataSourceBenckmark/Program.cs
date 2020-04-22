using System;
using BenchmarkDotNet.Running;

namespace DataSourceBenckmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Template>();
        }
    }
}
