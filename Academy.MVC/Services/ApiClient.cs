using System.Text.Json;

namespace Academy.MVC.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, cancellationToken);
        }
    }
}
