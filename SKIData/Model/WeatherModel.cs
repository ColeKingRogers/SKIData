using SKIData.Service;

namespace SKIData.Model
{
    public class WeatherModel
    {
        public Location location { get; set; }
        public CurrentWeather current { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string last_updated { get; set; }
        public double temp_f { get; set; }
        public double feelslike_f { get; set; }

    }
}
