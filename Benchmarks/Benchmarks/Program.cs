using System;
using System.IO;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GrapeCity.Documents.Excel;

namespace Benchmarks
{
    [RPlotExporter, RankColumn]
    public class DioDocsExcel
    {
        public static readonly byte[] Excel = File.ReadAllBytes("Report.xlsx");

        [Benchmark]
        public void Local()
        {
            ReportBuilder.Builder.Build();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DioDocsExcel>();
        }
    }
}
