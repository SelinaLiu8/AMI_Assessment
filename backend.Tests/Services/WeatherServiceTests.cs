using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using backend.Models;
using backend.Services;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handlerFunc;

    public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handlerFunc)
    {
        _handlerFunc = handlerFunc;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_handlerFunc(request));
    }
}


public class WeatherServiceForTest : WeatherService
{
    public WeatherServiceForTest(HttpClient client) : base(client) { }
}

public class WeatherServiceTests
{
    [Fact]
    public async Task GetWeatherDataAsync_ReturnsParsedWeatherResponse()
    {
        var fakeTokenResponse = new { accessToken = "fake-token" };
        var fakeWeatherData = new[]
        {
            new
            {
                temperature = 25.5,
                cloudCoverage = 40,
                windSpeed = 10,
                windDirection = 180,
                precipitationType = "rain",
                precipitationProbability = 0.4
            }
        };
        var fakeHighestTemps = new[]
        {
            new { rolling12MonthTemps = new double[] { 30, 31, 32 } }
        };

        var handler = new MockHttpMessageHandler(req =>
        {
            if (req.RequestUri!.ToString().Contains("AccessToken"))
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(fakeTokenResponse), Encoding.UTF8, "application/json")
                };
            if (req.RequestUri!.ToString().Contains("HighestTemps"))
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(fakeHighestTemps), Encoding.UTF8, "application/json")
                };
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(fakeWeatherData), Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(handler);
        var service = new WeatherServiceForTest(httpClient);

        var request = new WeatherRequest
        {
            City = "Ann Arbor",
            State = "MI",
            Zip = "48104",
            UnitOfMeasurement = "C"
        };

        var result = await service.GetWeatherDataAsync(request);

        Assert.Equal("Ann Arbor", result.City);
        Assert.Equal("MI", result.State);
        Assert.Equal(25.5, result.Temperature);
        Assert.Equal(3, result.Rolling12MonthTemps.Length);
    }

    [Fact]
    public async Task GetWeatherDataAsync_HandlesMissingFieldsGracefully()
    {
        var fakeWeatherData = new[]
        {
            new
            {
                temperature = (double?)null,
                cloudCoverage = 40,
                windSpeed = 0,
                windDirection = 0,
            }
        };
        var fakeHighestTemps = new[] { new { rolling12MonthTemps = Array.Empty<double>() } };

        var handler = new MockHttpMessageHandler(req =>
        {
            if (req.RequestUri!.ToString().Contains("AccessToken"))
            {
                var tokenResponse = new { accessToken = "fake-token" };
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(tokenResponse), Encoding.UTF8, "application/json")
                };
            }
            else if (req.RequestUri!.ToString().Contains("HighestTemps"))
            {
                var highestResponse = new[] { new { rolling12MonthTemps = Array.Empty<double>() } };
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(highestResponse), Encoding.UTF8, "application/json")
                };
            }
            else
            {
                var weatherResponse = new[]
                {
                    new { temperature = (double?)null, cloudCoverage = 40, windSpeed = 0, windDirection = 0 }
                };
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(weatherResponse), Encoding.UTF8, "application/json")
                };
            }
        });

        var httpClient = new HttpClient(handler);
        var service = new WeatherServiceForTest(httpClient);

        var request = new WeatherRequest { City = "Test", State = "TS", Zip = "00000", UnitOfMeasurement = "C" };

        var result = await service.GetWeatherDataAsync(request);

        // Assert default values for missing fields
        Assert.Equal(-1000, result.Temperature);
        Assert.Equal(40, result.CloudCoverage);
        Assert.Equal(0, result.WindSpeed);
        Assert.Equal(0, result.WindDirection);
        Assert.Empty(result.Rolling12MonthTemps);
    }

    [Fact]
    public async Task GetWeatherDataAsync_ReturnsCapitalizedCityState()
    {
        var fakeTokenResponse = new { accessToken = "fake-token" };
        var fakeWeatherData = new[]
        {
            new
            {
                temperature = 25.5,
                cloudCoverage = 40,
                windSpeed = 10,
                windDirection = 180,
                precipitationType = "rain",
                precipitationProbability = 0.4
            }
        };
        var fakeHighestTemps = new[]
        {
            new { rolling12MonthTemps = new double[] { 30, 31, 32 } }
        };

        var handler = new MockHttpMessageHandler(req =>
        {
            if (req.RequestUri!.ToString().Contains("AccessToken"))
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(fakeTokenResponse), Encoding.UTF8, "application/json")
                };
            if (req.RequestUri!.ToString().Contains("HighestTemps"))
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(fakeHighestTemps), Encoding.UTF8, "application/json")
                };
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(fakeWeatherData), Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(handler);
        var service = new WeatherServiceForTest(httpClient);

        var request = new WeatherRequest
        {
            City = "ann arbor",
            State = "mi",
            Zip = "48104",
            UnitOfMeasurement = "C"
        };

        var result = await service.GetWeatherDataAsync(request);

        Assert.Equal("Ann Arbor", result.City);
        Assert.Equal("MI", result.State);
        Assert.Equal(25.5, result.Temperature);
        Assert.Equal(3, result.Rolling12MonthTemps.Length);
    }

    [Fact]
    public async Task GetWeatherDataAsync_InvalidStateForCity_ReturnsBadRequest()
    {
        var fakeResponse = new
        {
            message = "Invalid state for city."
        };

        var handler = new MockHttpMessageHandler(req =>
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(fakeResponse), Encoding.UTF8, "application/json")
            };
        });

        var httpClient = new HttpClient(handler);
        var service = new WeatherServiceForTest(httpClient);

        var request = new WeatherRequest
        {
            City = "Ann Arbor",
            State = "TX",
            Zip = "48104",
            UnitOfMeasurement = "F"
        };

        var ex = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            await service.GetWeatherDataAsync(request)
        );

        Assert.Contains("400", ex.Message);
    }
}