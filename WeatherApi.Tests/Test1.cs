namespace WeatherApi.Tests;

using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;
using Moq.Protected;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

public class Test1
{
    [Theory]
    [InlineData(WeatherApi.Models.ApiParameters.AirTemperature)]
    [InlineData(WeatherApi.Models.ApiParameters.GustWindSpeed)]
    public async Task CallsCorrectParameter(string parameter)
    {
        var url = $"https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/{parameter}/station-set/all/period/latest-hour/data.json";
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var json = LoadTestData("all_latest_1.json");

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            });

        var client = new SmhiClientWrapper(new HttpClient(handlerMock.Object));
        var service = new WeatherService(client);

        var result = await service.GetWeatherDataAsync(parameter, null, null, null);
        handlerMock.Invocations.Count.Should().Be(1);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CanFilterOnStation()
    {
        var url = $"https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/1/station/97280/period/latest-months/data.json";
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var json = LoadTestData("97280_data.json");

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            });

        var client = new SmhiClientWrapper(new HttpClient(handlerMock.Object));
        var service = new WeatherService(client);

        var result = await service.GetWeatherDataAsync("1", "97280", null, null);
        handlerMock.Invocations.Count.Should().Be(1);

        result.Should().HaveCount(5);
        result.First().Should().BeEquivalentTo(new ObservationDto { Date = DateTimeOffset.FromUnixTimeMilliseconds(1745802000000).LocalDateTime, StationId = "97280", StationName = "Adelsö A", Value = 9.7 });
    }

    [Fact]
    public async Task CanFilterOnStationAndDate()
    {
        var url = $"https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/1/station/97280/period/latest-months/data.json";
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var json = LoadTestData("97280_data.json");

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            });

        var client = new SmhiClientWrapper(new HttpClient(handlerMock.Object));
        var service = new WeatherService(client);

        var result = await service.GetWeatherDataAsync("1", "97280", new DateTime(2025, 04, 28), new DateTime(2025, 04, 28));
        handlerMock.Invocations.Count.Should().Be(1);

        result.Should().HaveCount(4);

        result.First().Should().BeEquivalentTo(new ObservationDto { Date = DateTimeOffset.FromUnixTimeMilliseconds(1745802000000).LocalDateTime, StationId = "97280", StationName = "Adelsö A", Value = 9.7 });

        handlerMock.Invocations.Clear();
        result = await service.GetWeatherDataAsync("1", "97280", new DateTime(2025, 04, 28), null);
        result.Should().HaveCount(5);
        result.Last().Should().BeEquivalentTo(new ObservationDto { Date = DateTimeOffset.FromUnixTimeMilliseconds(1755812800000).LocalDateTime, StationId = "97280", StationName = "Adelsö A", Value = 19.4 });
    }

    private string LoadTestData(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", fileName);
        return File.ReadAllText(path);
    }
}
