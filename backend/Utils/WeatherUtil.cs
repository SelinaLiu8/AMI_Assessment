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

            public static double? GetDoubleSafe(JsonElement element, string propertyName)
            {
                try
                {
                    if (!element.TryGetProperty(propertyName, out var prop))
                        throw new KeyNotFoundException($"Property '{propertyName}' not found in the JSON element.");

                    if (prop.ValueKind != JsonValueKind.Number)
                        throw new InvalidCastException($"Property '{propertyName}' is not a number. Found: {prop.ValueKind}");

                    return prop.GetDouble();
                }
                catch (Exception ex) when (ex is InvalidCastException || ex is KeyNotFoundException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new JsonException($"Unexpected error reading property '{propertyName}'.", ex);
                }
            }
    }
}
