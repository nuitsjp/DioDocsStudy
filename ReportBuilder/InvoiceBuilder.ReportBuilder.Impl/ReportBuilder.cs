using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InvoiceBuilder.ReportBuilder.Impl
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly string _endpoint;

        private readonly HttpClient _httpClient = new HttpClient();

        public ReportBuilder(string endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task<byte[]> Build(object o)
        {
            var json = JsonConvert.SerializeObject(o);
            var response = await _httpClient.PostAsync(_endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
            return null;
        }
    }
}