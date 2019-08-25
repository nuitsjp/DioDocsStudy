using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
            using(var stream = new MemoryStream(Excel))
            {
                ReportBuilder.Builder.Build(stream);
            }
        }

        private static readonly HttpClient httpClient = new HttpClient();

        [Benchmark]
        public void Premium()
        {
            httpClient.GetAsync("https://ddbench-linux-premium.azurewebsites.net/api/CreatePdf?code=QZOW34LY/2aAIEj/pVtVcjzVVf9UGyIhyzUHwFMSj3ibPvNf01MkTg==").GetAwaiter().GetResult();
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
