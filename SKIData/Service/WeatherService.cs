using RestSharp;
using SKIData.Model;
using System;
using System.Configuration;
using System.Diagnostics;


namespace SKIData.Service
{
    public class WeatherService
    {
        private readonly string key = "9da6a1e0f99149339de204217252010";

        public async Task<WeatherModel> GetRealTimeWeather()
        {
            WeatherModel weather = new WeatherModel();
            string url = "https://api.weatherapi.com/v1";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("My C# API Client");
                HttpResponseMessage response = await client.GetAsync(url, key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching weather data: {ex.Message}");
            }


            return weather;
        }
    }
}
