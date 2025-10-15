namespace backend.Models
{
    public class Precipitation
    {
        public string Type { get; set; } = "";
        public double Probability { get; set; }
    }
    public class WeatherResponse
    {
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zip { get; set; } = "";
        public string UnitMeasure { get; set; } = "";

        public double? Temperature { get; set; }
        public double? CloudCoverage { get; set; }
        public double? WindSpeed { get; set; }
        public double? WindDirection { get; set; }
        public double[] Rolling12MonthTemps { get; set; } = Array.Empty<double>();
        public Precipitation[] Precipitation { get; set; } = Array.Empty<Precipitation>();
    }
}
