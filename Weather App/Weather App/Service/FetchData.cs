using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App.Service
{
    public class FetchData
    {
        private double latitude;
        private double longitude;

        public FetchData( double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public async Task<string> GetWeatherData()
        {

            string body = null;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.openweathermap.org/data/2.5/onecall?" +
                $"lat={latitude}&lon={longitude}&appid=0d057eb24363f49cb920c198940ec466")
               
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