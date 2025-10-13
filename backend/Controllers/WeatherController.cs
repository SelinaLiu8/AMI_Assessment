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

            var result = await _weatherService.GetWeatherDataAsync(request);
            return Ok(result);
        }
    }
}