using WeatherApi.Models;
using WeatherApi.Services;

public class WeatherService
{
    private readonly SmhiClientWrapper client;

    public WeatherService(SmhiClientWrapper client)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<IEnumerable<ObservationDto>> GetWeatherDataAsync(string parameterId, string? stationId, DateTime? startDate, DateTime? endDate)
    {
        // Base call, no filtering, just get latest data for all stations
        if (startDate == null && endDate == null && stationId == null)
        {
            return (await client.GetLatestAsync(parameterId)).ToList();
        }

        // Filtering by only station
        if (startDate == null && endDate == null && stationId != null)
        {
            return (await client.GetStationAsync(parameterId, stationId)).ToList();
        }

        // Filtering by station and date range
        if (stationId != null)
        {
            return (await client.GetStationAsync(parameterId, stationId)).ToList().Where(o =>
                (startDate == null || o.Date >= startDate) &&
                (endDate == null || o.Date <= endDate));
        }

        throw new NotImplementedException("Fetching historical data is not implemented yet.");
    }
}
