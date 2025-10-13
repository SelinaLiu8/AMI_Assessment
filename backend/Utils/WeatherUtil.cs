using System;
using System.Linq;
using System.Text.Json;
using backend.Models;

namespace backend.Utils
{
    public static class WeatherUtil
    {
        public static string CapitalizeWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return string.Join(" ", input.Split(' ')
                                         .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));
        }

        public static string UpperCaseOrDefault(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Trim().ToUpper();
        }

        public static Precipitation[] MapPrecipitation(JsonElement weatherData)
        {
            if (weatherData.TryGetProperty("precipitation", out var precipArray))
            {
                return precipArray.EnumerateArray()
                    .Select(p => new Precipitation
                    {
                        Type = p.GetProperty("type").GetString() ?? "",
                        Probability = p.GetProperty("probability").GetDouble()
                    })
                    .ToArray();
            }

            return Array.Empty<Precipitation>();
        }
    }
}
