using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather_App.Service
{
    public class FetchData
    {
        private double latitude;
        private double longitude;
        private string api_key = "0d057eb24363f49cb920c198940ec466";

        public FetchData(double latitude, double longitude)
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
                $"lat={latitude}&lon={longitude}&appid={api_key}")

            };
            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        body = await response.Content.ReadAsStringAsync();
                    }
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