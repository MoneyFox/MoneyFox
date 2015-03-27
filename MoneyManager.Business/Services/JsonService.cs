using System;
using System.Net.Http;
using System.Threading.Tasks;
using MoneyManager.Foundation.OperationContracts;
using Xamarin;

namespace MoneyManager.Business.Services {
    public class JsonService : IJsonService {
        private static HttpClient _httpClient = new HttpClient();

        public async Task<string> GetJsonFromService(string url) {
            try {
                PrepareHttpClient();
                var req = new HttpRequestMessage(HttpMethod.Get, url);
                HttpResponseMessage response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                Insights.Report(ex, ReportSeverity.Error);
            }
            return "1";
        }

        private void PrepareHttpClient() {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.SmallInvoice.com/") };
            _httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }
    }
}
