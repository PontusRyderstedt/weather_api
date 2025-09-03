namespace WeatherApi.Models;

public class ObservationDto
{
    public string StationId { get; set; } = "";
    public string StationName { get; set; } = "";
    public DateTime Date { get; set; }
    public double? Value { get; set; }
}