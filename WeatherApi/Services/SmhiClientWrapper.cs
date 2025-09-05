using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class SmhiClientWrapper : IDisposable
    {
        private readonly string BaseUrl = "https://opendata-download-metobs.smhi.se/api";
        private readonly string ApiVersion = "/version/1.0";
        private readonly HttpClient httpClient;

        public SmhiClientWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private string LatestUrl(string parameterId) =>
            $"{BaseUrl}{ApiVersion}/parameter/{parameterId}/station-set/all/period/latest-hour/data.json";

        private string StationUrl(string parameterId, string stationId) =>
            $"{BaseUrl}{ApiVersion}/parameter/{parameterId}/station/{stationId}/period/latest-months/data.json";

        private string HistoricalUrl(string parameterId, string stationId, DateTime? startDate, DateTime? endDate) =>
            $"{BaseUrl}{ApiVersion}/parameter/{parameterId}/station/{stationId}/period/latest-months/data.json";

        public async Task<SmhiStationsResponse?> GetLatestAsync(string parameterId)
        {
            var url = LatestUrl(parameterId);
            var response = await httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<SmhiStationsResponse>(content);
            return jsonData;
        }

        public async Task<SmhiSingleStationResponse?> GetStationAsync(string parameterId, string stationId)
        {
            var url = StationUrl(parameterId, stationId);
            var response = await httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jsonData = JsonSerializer.Deserialize<SmhiSingleStationResponse>(content);
            return jsonData;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}