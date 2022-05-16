using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App.Service
{
    public class FetchData
    {
        private string location;
        private double latitude;
        private double longitude;

        public FetchData(string location, double latitude, double longitude)
        {
            this.location = location;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public async Task<string> WeatherData2()
        {

            string body = null;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.openweathermap.org/data/2.5/onecall?lat=23.8962&lon=29.4486&appid=0d057eb24363f49cb920c198940ec466")
                //Headers =
                //{
                //    { "X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com" },
                //    { "X-RapidAPI-Key", "a8f1121782msh47677863eeab683p19d625jsn1fa9d9f59efc" },
                //},
            };
            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    body = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return body;
        }


        public async Task<string> WeatherData()
        {
            var client = new HttpClient();
            string body = null;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://community-open-weather-map.p.rapidapi.com/forecast?q=Polokwane%2C%20ZA"),
                Headers =
                {
                    { "X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com" },
                    { "X-RapidAPI-Key", "a8f1121782msh47677863eeab683p19d625jsn1fa9d9f59efc" },
                },
            };
            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    body = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return body;
        }
    }

}