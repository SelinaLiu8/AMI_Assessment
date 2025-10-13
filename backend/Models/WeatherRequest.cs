using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class WeatherRequest
    {
        [Required]
        public string City { get; set; } = string.Empty;

        [Required, StringLength(2, MinimumLength = 2)]
        public string State { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{5}$")]
        public string Zip { get; set; } = string.Empty;

        [RegularExpression(@"^[CF]$", ErrorMessage = "Unit must be 'C' or 'F'.")]
        public string? UnitOfMeasurement { get; set; } = string.Empty;
    }
}