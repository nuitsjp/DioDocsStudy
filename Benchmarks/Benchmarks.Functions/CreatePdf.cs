using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Benchmarks.Functions
{
    public static class CreatePdf
    {
        [FunctionName("CreatePdf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("templates/Report.xlsx", FileAccess.Read)]Stream input,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. input:{input}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            ReportBuilder.Builder.Build(input);
            stopwatch.Stop();
            return new OkObjectResult(stopwatch.Elapsed);
        }
    }
}
