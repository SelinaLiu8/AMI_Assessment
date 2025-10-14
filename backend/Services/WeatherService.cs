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

        // public WeatherService()
        // {
        //     _httpClient = new HttpClient();
        // }

        // For testing only
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Get current weather
            var weatherResponse = await _httpClient.PostAsync(
                "https://ami-interviewassessment.azurewebsites.net/WeatherData/ByLocation",
                content);
            weatherResponse.EnsureSuccessStatusCode();
            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
            var weatherData = JsonDocument.Parse(weatherJson).RootElement[0];

            // Helper to safely get a double or null
            double? GetDoubleSafe(JsonElement element, string propertyName)
            {
                if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Number)
                    return prop.GetDouble();
                return null;
            }

            // Get 12-month highest temps
            var highestResponse = await _httpClient.PostAsync(
                "https://ami-interviewassessment.azurewebsites.net/WeatherData/ByLocation/HighestTemps",
                content);
            highestResponse.EnsureSuccessStatusCode();
            var highestJson = await highestResponse.Content.ReadAsStringAsync();
            var highestData = JsonDocument.Parse(highestJson).RootElement[0];

            return new WeatherResponse
            {
                City = city,
                State = state,
                Zip = request.Zip,
                UnitMeasure = request.UnitOfMeasurement,
                Temperature = GetDoubleSafe(weatherData, "temperature") ?? -1000,
                CloudCoverage = GetDoubleSafe(weatherData, "cloudCoverage") ?? -1000,
                WindSpeed = GetDoubleSafe(weatherData, "windSpeed") ?? -1000,
                WindDirection = GetDoubleSafe(weatherData, "windDirection") ?? -1000,
                Rolling12MonthTemps = highestData.TryGetProperty("rolling12MonthTemps", out var arr)
                    ? arr.EnumerateArray().Select(e => e.GetDouble()).ToArray()
                    : Array.Empty<double>(),
                Precipitation = WeatherUtil.MapPrecipitation(weatherData)
            };
        }
    }
}
