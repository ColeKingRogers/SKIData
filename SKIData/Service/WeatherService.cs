using RestSharp;
using SKIData.Model;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace SKIData.Service
{
    public class WeatherService
    {
        private readonly string key;

        public WeatherService(IConfiguration config)
        {
            key = config["ApiSettings:Apikey"];
        }

        public async Task<WeatherModel> GetRealTimeWeather(string q) //making the API call for real-time weather
        {
            WeatherModel weather = new WeatherModel();
            string url = "https://api.weatherapi.com/v1/current.json?q=" + q + "&key=" + key;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<WeatherResponse>();
                    weather = new WeatherModel //mapping the data to the WeatherModel
                    {
                        region = data.location.region,
                        country = data.location.country,
                        last_updated = data.current.last_updated,
                        temp_f = (int)data.current.temp_f,
                        feelslike_f = data.current.feelslike_f
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching weather data: {ex.Message}");
            }


            return weather;
        }
    }
    public class WeatherResponse //all of these classes are for deserializing the JSON response
    {
        public Location location { get; set; }
        public CurrentWeather current { get; set; }
    }

    public class Location
    {
        public string region { get; set; }
        public string country { get; set; }
    }

    public class CurrentWeather
    {
        public string last_updated { get; set; }
        public double temp_f { get; set; }
        public double feelslike_f { get; set; }
    }
}
