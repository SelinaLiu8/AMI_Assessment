using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather([FromBody] WeatherRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            try {
                var result = await _weatherService.GetWeatherDataAsync(request);
                return Ok(result);
            } 
            catch (HttpRequestException ex)
            {
                // Handles network or 4xx/5xx response issues
                return BadRequest($"Error fetching weather data: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Generic fallback for any unexpected error
                throw new Exception($"Unexpected error occurred: {ex.Message}", ex);
            }
            
            
        }
    }
}