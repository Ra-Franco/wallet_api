using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace WebApi.Test
{
    public class WalletCustomClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        public WalletCustomClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        protected async Task<HttpResponseMessage> DoPost(string route, object request, string culture = "en", string token = "")
        {
            ChangeRequestCulture(culture);
            AuthrizeRequest(token);

            var json = System.Text.Json.JsonSerializer.Serialize(request);
            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(route, content);
        }
        protected async Task<HttpResponseMessage> DoGet(string route, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthrizeRequest(token);

            return await _httpClient.GetAsync(route);
        }

        protected async Task<HttpResponseMessage> DoDelete(string route, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthrizeRequest(token);

            return await _httpClient.DeleteAsync(route);
        }

        protected async Task<HttpResponseMessage> DoPut(string route, object request, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthrizeRequest(token);

            return await _httpClient.PutAsJsonAsync(route, request);
        }

        protected async Task<HttpResponseMessage> DoPatch(string route, object request, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthrizeRequest(token);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PatchAsync(route, content);
        }
        private void ChangeRequestCulture(string culture)
        {

            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

        private void AuthrizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Remove("Authorization");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
