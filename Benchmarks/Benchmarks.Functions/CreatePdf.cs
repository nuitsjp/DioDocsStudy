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
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;
using System.Text;

namespace Benchmarks.Functions
{
    public static class CreatePdf
    {
        [FunctionName("CreatePdf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("templates/Report.xlsx", FileAccess.Read)]Stream input,
            [Blob("output", FileAccess.Write)] CloudBlobContainer outputContainer,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. input:{input}");

            await outputContainer.CreateIfNotExistsAsync();
            var cloudBlockBlob = outputContainer.GetBlockBlobReference("result.pdf");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using(var output = await cloudBlockBlob.OpenWriteAsync())
            {
                ReportBuilder.Builder.Build(input, output);
            }
            stopwatch.Stop();
            return new OkObjectResult(stopwatch.Elapsed);
        }
    }
}
