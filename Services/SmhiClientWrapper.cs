using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class SmhiClientWrapper : IDisposable
    {
        private readonly HttpClient httpClient;

        public SmhiClientWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<SmhiStationResponse?> GetAsync(string requestUri)
        {
            var response = await httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<SmhiStationResponse>(content);
            return jsonData;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}