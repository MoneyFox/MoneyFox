using System;
using System.Net.Http;
using System.Threading.Tasks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Services {
    public class JsonService : IJsonService {
        private HttpClient _httpClient = new HttpClient();
        private readonly string _url;

        /// <summary>
        /// Creates an instance of JsonService
        /// </summary>
        /// <param name="url">URL for the service to retrieve the json.</param>
        public JsonService(string url) {
            _url = url;
        }

        /// <summary>
        /// Return a JSON string from the instanced service
        /// </summary>
        /// <returns>Recived JSON string.</returns>
        public async Task<string> GetJsonFromService() {
            try {
                PrepareHttpClient();
                var req = new HttpRequestMessage(HttpMethod.Get, _url);
                HttpResponseMessage response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                InsightHelper.Report(ex);
            }
            return "1";
        }

        private void PrepareHttpClient() {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }
    }
}
