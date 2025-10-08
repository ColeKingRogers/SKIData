namespace SKIData.Model
{
    public class WeatherModel
    {
        public string City { get; set; }
        public string Condition { get; set; }
        public double TemperatureCelsius { get; set; }
        public double WindSpeedKph { get; set; }
        public double SnowDepthCm { get; set; }
    }
}
