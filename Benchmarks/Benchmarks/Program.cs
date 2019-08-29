using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GrapeCity.Documents.Excel;

namespace Benchmarks
{
    [RPlotExporter, RankColumn]
    public class ResponseTime
    {
        public static readonly byte[] Excel = File.ReadAllBytes("Report.xlsx");

        private void Execute(Action action) => action();

        private void Execute(string url) => Execute(() => httpClient.GetAsync(url).GetAwaiter().GetResult());

        [Benchmark]
        public void Local()
        {
            Execute(() =>
            {
                using (var stream = new MemoryStream(Excel))
                {
                    ReportBuilder.Builder.Build(stream, Stream.Null);
                }
            });
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
            Execute("https://ddbench-win-consumption.azurewebsites.net/api/createpdfforstreamnull?code=oKY/ZOnbiAGCWeXPs54uIiW4BDyCfajF4S4GukO633H6alHTB81hNg==");
        }

        [Benchmark]
        public void WinS1()
        {
            Execute("https://ddbench-win-s1.azurewebsites.net/api/createpdfforstreamnull?code=iaIucEyDC2RSgDDYEa/OvmSVUCWxvjOqkGR6fVN0py8uTme7ejEq5w==");
        }

        [Benchmark]
        public void WinS2()
        {
            Execute("https://ddbench-win-s2.azurewebsites.net/api/createpdfforstreamnull?code=7sgqEu3u0v3kScFMqo6NR5MTsh4UL0sQcEuAGJVmhmWSWwZRY45ZUA==");
        }

        [Benchmark]
        public void WinS3()
        {
            Execute("https://ddbench-win-s3.azurewebsites.net/api/createpdfforstreamnull?code=2c3IzmMaCs8ia73SJh4lZ4kmZyn7IPd8BQZ2vfNiDH4OmUKxQhzHbw==");
        }

        [Benchmark]
        public void WinEP1()
        {
            Execute("https://ddbench-win-ep1.azurewebsites.net/api/createpdfforstreamnull?code=ofm1LImfHpr0ukicutXCKBTwyGykdvCdJnPgrL2zVxB2RfaL78MKkw==");
        }

        [Benchmark]
        public void WinEP2()
        {
            Execute("https://ddbench-win-ep2.azurewebsites.net/api/createpdfforstreamnull?code=1jMDHCONtEGukao8v8IDxLh4qbBVwpaj4/dmL7tcpdvvgdU0CfQ3AQ==");
        }

        [Benchmark]
        public void WinEP3()
        {
            Execute("https://ddbench-win-ep3.azurewebsites.net/api/createpdfforstreamnull?code=MI3Y2DlENzUQftOCjgIfbJJ25xyLzjHYevZKOaRinsK6vWL6xsb2Mg==");
        }

    }

    [RPlotExporter, RankColumn]
    public class Throughput
    {
        public static readonly byte[] Excel = File.ReadAllBytes("Report.xlsx");

        [Params(10, 100)]
        public int N { get; set; }

        private void Execute(Action action)
        {
            Parallel.ForEach(Enumerable.Range(1, N), x => action());
        }

        private void Execute(string url) => Execute(() => httpClient.GetAsync(url).GetAwaiter().GetResult());

        //[Benchmark]
        //public void Local()
        //{
        //    Parallel.ForEach(
        //        Enumerable.Range(1, N),
        //        _ =>
        //        {
        //            using (var stream = new MemoryStream(Excel))
        //            {
        //                ReportBuilder.Builder.Build(stream, Stream.Null);
        //            }
        //        });
        //}

        private static readonly HttpClient httpClient = new HttpClient();

        //[Benchmark]
        //public void LinuxPremium()
        //{
        //    httpClient.GetAsync("https://ddbench-linux-premium.azurewebsites.net/api/CreatePdf?code=QZOW34LY/2aAIEj/pVtVcjzVVf9UGyIhyzUHwFMSj3ibPvNf01MkTg==").GetAwaiter().GetResult();
        //}

        //[Benchmark]
        //public void WinConsumption()
        //{
        //    Execute("https://ddbench-win-consumption.azurewebsites.net/api/createpdfforstreamnull?code=oKY/ZOnbiAGCWeXPs54uIiW4BDyCfajF4S4GukO633H6alHTB81hNg==");
        //}

        //[Benchmark]
        //public void WinS1()
        //{
        //    Execute("https://ddbench-win-s1.azurewebsites.net/api/createpdfforstreamnull?code=iaIucEyDC2RSgDDYEa/OvmSVUCWxvjOqkGR6fVN0py8uTme7ejEq5w==");
        //}

        //[Benchmark]
        //public void WinS2()
        //{
        //    Execute("https://ddbench-win-s2.azurewebsites.net/api/createpdfforstreamnull?code=7sgqEu3u0v3kScFMqo6NR5MTsh4UL0sQcEuAGJVmhmWSWwZRY45ZUA==");
        //}

        //[Benchmark]
        //public void WinS3()
        //{
        //    Execute("https://ddbench-win-s3.azurewebsites.net/api/createpdfforstreamnull?code=2c3IzmMaCs8ia73SJh4lZ4kmZyn7IPd8BQZ2vfNiDH4OmUKxQhzHbw==");
        //}

        //[Benchmark]
        //public void WinEP1()
        //{
        //    Execute("https://ddbench-win-ep1.azurewebsites.net/api/createpdfforstreamnull?code=ofm1LImfHpr0ukicutXCKBTwyGykdvCdJnPgrL2zVxB2RfaL78MKkw==");
        //}

        //[Benchmark]
        //public void WinEP2()
        //{
        //    Execute("https://ddbench-win-ep2.azurewebsites.net/api/createpdfforstreamnull?code=1jMDHCONtEGukao8v8IDxLh4qbBVwpaj4/dmL7tcpdvvgdU0CfQ3AQ==");
        //}

        [Benchmark]
        public Task WinEP3()
        {
            var tasks = new List<Task>();
            Enumerable
                .Range(1, N)
                .ToList()
                .ForEach(_ =>
                {
                    var task = httpClient.GetAsync("https://ddbench-win-ep3.azurewebsites.net/api/CreatePdfForStreamNull?code=MI3Y2DlENzUQftOCjgIfbJJ25xyLzjHYevZKOaRinsK6vWL6xsb2Mg==");
                    tasks.Add(task);
                });
            return Task.WhenAll(tasks);
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<ResponseTime>();
            var summary = BenchmarkRunner.Run<Throughput>();
        }
    }
}
