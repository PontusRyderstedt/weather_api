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
    DateTime? from,
    DateTime? to) =>
{
    var data = await service.GetWeatherDataAsync(ApiParameters.GustWindSpeed, station, from, to);
    return data is null ? Results.NotFound() : Results.Ok(data);
});

app.MapGet("/weather/temperature", async (
    WeatherService service,
    string? station,
    DateTime? from,
    DateTime? to) =>
{
    var data = await service.GetWeatherDataAsync(ApiParameters.AirTemperature, station, from, to);
    return data is null ? Results.NotFound() : Results.Ok(data);
});

app.Run();
