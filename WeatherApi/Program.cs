using WeatherApi.Models;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<SmhiClientWrapper>();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

app.MapGet("/", () => "SMHI Weather API is running 🚀");

app.MapGet("/weather/wind", async (
    WeatherService service,
    string? station,
    string? from,
    string? to) =>
{
    var (stationId, fromDate, toDate, error) = Utils.ParseAndValidate(station, from, to);
    if (error != null) return error;

    var data = await service.GetWeatherDataAsync(ApiParameters.GustWindSpeed, stationId, fromDate, toDate);
    return data is null ? Results.NotFound() : Results.Ok(data);
});

app.MapGet("/weather/temperature", async (
    WeatherService service,
    string? station,
    string? from,
    string? to) =>
{
    var (stationId, fromDate, toDate, error) = Utils.ParseAndValidate(station, from, to);
    if (error != null) return error;

    var data = await service.GetWeatherDataAsync(ApiParameters.AirTemperature, stationId, fromDate, toDate);
    return data is null ? Results.NotFound() : Results.Ok(data);
});

app.Run();
