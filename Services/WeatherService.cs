using WeatherApi.Models;
using WeatherApi.Services;

public class WeatherService
{
    private readonly string BaseUrl = "https://opendata-download-metobs.smhi.se/api";
    private readonly string ApiVersion = "/version/1.0";
    private readonly SmhiClientWrapper client;

    public WeatherService(SmhiClientWrapper client)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    private string LatestUrl(string parameterId) =>
        $"{BaseUrl}{ApiVersion}/parameter/{parameterId}/station-set/all/period/latest-hour/data.json";

    public async Task<SmhiStationResponse?> GetWeatherDataAsync(string parameterId, string? stationId, DateTime? startDate, DateTime? endDate)
    {
        if (startDate == null && endDate == null && stationId == null)
        {
            return await GetLatestAsync(parameterId);
        }

        throw new NotImplementedException("Fetching historical data is not implemented yet.");
    }
    
    private async Task<SmhiStationResponse?> GetLatestAsync(string parameterId)
    {
        var response = await client.GetAsync(LatestUrl(parameterId));
        return response;
    }
}
