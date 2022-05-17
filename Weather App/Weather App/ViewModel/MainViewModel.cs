using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather_App.Service;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather_App.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Daily> daily = new ObservableCollection<Daily>();
        public ObservableCollection<Daily> Daily { get { return daily; } set { daily = value; } }
        private ObservableCollection<Current> hourly = new ObservableCollection<Current>();
        private ObservableCollection<Current> Hourly { get { return hourly; } set { hourly = value; } }
        private string temperature;
        private string location;
        private string icon;
        private string description;
        private string searchInput;
        private string pressure;
        private string cloudness;
        private string humidity;
        private string wind;

        public string Pressure { get { return pressure; } set { pressure = value; PropertyChanged(this, new PropertyChangedEventArgs("Pressure")); } }
        public string Cloudness {
            get
            {
                return cloudness;
            }
            set
            {
                cloudness = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Cloudness"));
            }
        }
        public string Humidity
        {
            get
            {
                return humidity;
            }
            set
            {
                humidity = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Humidity"));
            }
        }
        public string Wind
        {
            get
            {
                return wind;
            }
            set
            {
                wind = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Wind"));
            }
        }

        public string Icon { get { return icon; }
            set { icon = value; PropertyChanged(this, new PropertyChangedEventArgs("Icon")); } }
        public string Location { get { return location; } set { location = value; PropertyChanged(this, new PropertyChangedEventArgs("Location")); } }
        public string Temperature { get { return temperature; } set { temperature = value; PropertyChanged(this, new PropertyChangedEventArgs("Temperature")); } }
        public string Description { get { return description; } set { description = value; PropertyChanged(this, new PropertyChangedEventArgs("Description")); } }
        public string SearchInput { get { return searchInput; } set { searchInput = value; PropertyChanged(this, new PropertyChangedEventArgs("SearchInput")); } }
        

        public ICommand BtnFetchData { get; set; }
        public ICommand BtnSearch { get; set; }

        public MainViewModel()
        {
            
            BtnSearch = new Command(SearchLocation);
            _ = CurrentLocation();
        }


        private const double kelvin = 273.15;
        public async void LoadData(double latitude, double longitude)
        {
            
            FetchData data = new FetchData(latitude, longitude);
            var json = await data.GetWeatherData();
            Console.WriteLine(json);

            if(json != null)
            {
                var output = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(json);
                Temperature = $"{(long)(output.Current.Temp - kelvin)}";
                Humidity = $"{output.Current.Humidity}%";
                Cloudness = $"{output.Current.Clouds}%";
                Pressure = $"{output.Current.Pressure} hPa";
                Wind = $"{output.Current.WindSpeed} m/h";
                Icon = $"https://openweathermap.org/img/wn/{output.Current.Weather[0].Icon}@4x.png";
                
                Description = output.Current.Weather[0].Description;
                daily.Clear();
                foreach (var item in output.Daily)
                {
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(item.Dt).ToLocalTime();
                    item.Day = dateTime.DayOfWeek;
                    if (DateTime.Now.Date != dateTime.Date)
                    {
                        item.Temp.Min = ((long)(item.Temp.Min - kelvin));
                        daily.Add(item);
                    }
                   
                }
                hourly.Clear();
                foreach (var item in output.Hourly)
                {
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(item.Dt).ToLocalTime();
                    
                    hourly.Add(item);
                    Console.WriteLine("weeee " + dateTime.Hour);
                }
            }

        }
        private async Task CurrentLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            var result = await Geocoding.GetPlacemarksAsync(location);
            var addr = result.FirstOrDefault();
            
            if(addr !=null)
            {
                Location = $"{addr.SubLocality}, {addr.CountryCode}";
                SearchInput = addr.SubLocality;
                LoadData(addr.Location.Latitude, addr.Location.Longitude);
            }
        }
        private async void SearchLocation(object obj)
        {
            if(searchInput != null)
            {

                var results = await Xamarin.Essentials.Geocoding.GetLocationsAsync(SearchInput);
                var place = results.FirstOrDefault();
                if(place != null)
                {
                    Location = searchInput;
                    LoadData(place.Latitude, place.Longitude);
                }
            }
        }

    }
}
