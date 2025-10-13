using backend.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend.Utils;

namespace backend.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var tokenUrl = "https://ami-interviewassessment.azurewebsites.net/Auth/AccessToken";
            var response = await _httpClient.GetAsync(tokenUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("accessToken").GetString()!;
        }

        public async Task<WeatherResponse> GetWeatherDataAsync(WeatherRequest request)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var city = WeatherUtil.CapitalizeWords(request.City);
            var state = WeatherUtil.UpperCaseOrDefault(request.State);

            var payload = new
            {
                locations = new[]
                {
                    new { city = city, state = state, zip = request.Zip }
                },
                unitOfMeasurement = request.UnitOfMeasurement
            };

            var json = JsonSerializer.Serialize(payload);
            Console.WriteLine("Payload: " + json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Get current weather
            var weatherResponse = await _httpClient.PostAsync(
                "https://ami-interviewassessment.azurewebsites.net/WeatherData/ByLocation",
                content);
            weatherResponse.EnsureSuccessStatusCode();
            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
            var weatherData = JsonDocument.Parse(weatherJson).RootElement[0];

            // Get 12-month highest temps
            var content2 = new StringContent(json, Encoding.UTF8, "application/json");
            var highestResponse = await _httpClient.PostAsync(
                "https://ami-interviewassessment.azurewebsites.net/WeatherData/ByLocation/HighestTemps",
                content2);
            highestResponse.EnsureSuccessStatusCode();
            var highestJson = await highestResponse.Content.ReadAsStringAsync();
            var highestData = JsonDocument.Parse(highestJson).RootElement[0];

            return new WeatherResponse
            {
                City = city,
                State = state,
                Zip = request.Zip,
                UnitMeasure = request.UnitOfMeasurement,
                Temperature = weatherData.GetProperty("temperature").GetDouble(),
                CloudCoverage = weatherData.GetProperty("cloudCoverage").GetDouble(),
                WindSpeed = weatherData.GetProperty("windSpeed").GetDouble(),
                WindDirection = weatherData.GetProperty("windDirection").GetDouble(),
                Rolling12MonthTemps = highestData.GetProperty("rolling12MonthTemps")
                    .EnumerateArray()
                    .Select(e => e.GetDouble())
                    .ToArray(),
                Precipitation = WeatherUtil.MapPrecipitation(weatherData)
            };
        }
    }
}
