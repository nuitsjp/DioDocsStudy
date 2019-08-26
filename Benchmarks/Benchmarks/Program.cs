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
                ReportBuilder.Builder.Build(stream, Stream.Null);
            }
        }

        private static readonly HttpClient httpClient = new HttpClient();

        //[Benchmark]
        //public void LinuxPremium()
        //{
        //    httpClient.GetAsync("https://ddbench-linux-premium.azurewebsites.net/api/CreatePdf?code=QZOW34LY/2aAIEj/pVtVcjzVVf9UGyIhyzUHwFMSj3ibPvNf01MkTg==").GetAwaiter().GetResult();
        //}

        [Benchmark]
        public void WinConsumption()
        {
            httpClient.GetAsync("https://ddbench-win-consumption.azurewebsites.net/api/createpdfforstreamnull?code=oKY/ZOnbiAGCWeXPs54uIiW4BDyCfajF4S4GukO633H6alHTB81hNg==").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void WinS1()
        {
            httpClient.GetAsync("https://ddbench-win-s1.azurewebsites.net/api/createpdfforstreamnull?code=iaIucEyDC2RSgDDYEa/OvmSVUCWxvjOqkGR6fVN0py8uTme7ejEq5w==").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void WinS2()
        {
            httpClient.GetAsync("https://ddbench-win-s2.azurewebsites.net/api/createpdfforstreamnull?code=7sgqEu3u0v3kScFMqo6NR5MTsh4UL0sQcEuAGJVmhmWSWwZRY45ZUA==").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void WinS3()
        {
            httpClient.GetAsync("https://ddbench-win-s3.azurewebsites.net/api/createpdfforstreamnull?code=2c3IzmMaCs8ia73SJh4lZ4kmZyn7IPd8BQZ2vfNiDH4OmUKxQhzHbw==").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void WinEP1()
        {
            httpClient.GetAsync("https://ddbench-win-ep1.azurewebsites.net/api/createpdfforstreamnull?code=ofm1LImfHpr0ukicutXCKBTwyGykdvCdJnPgrL2zVxB2RfaL78MKkw==").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void WinEP2()
        {
            httpClient.GetAsync("https://ddbench-win-ep2.azurewebsites.net/api/createpdfforstreamnull?code=1jMDHCONtEGukao8v8IDxLh4qbBVwpaj4/dmL7tcpdvvgdU0CfQ3AQ==").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void WinEP3()
        {
            httpClient.GetAsync("https://ddbench-win-ep3.azurewebsites.net/api/createpdfforstreamnull?code=MI3Y2DlENzUQftOCjgIfbJJ25xyLzjHYevZKOaRinsK6vWL6xsb2Mg==").GetAwaiter().GetResult();
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
