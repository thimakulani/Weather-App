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
        public ObservableCollection<Current> Hourly { get { return hourly; } set { hourly = value; } }
        private long temperature;
        private string location;
        private string icon;
        private string description;
        private string searchInput;
        private long pressure;
        private long cloudness;
        private long humidity;
        private double wind;
        private string sunrise;
        private string sunset;

        public string Sunset { get { return sunset; } set { sunset = value; PropertyChanged(this, new PropertyChangedEventArgs("Sunset")); } }
        public string Sunrise { get { return sunrise; } set { sunrise = value; PropertyChanged(this, new PropertyChangedEventArgs("Sunrise")); } }
        public long Pressure { get { return pressure; } set { pressure = value; PropertyChanged(this, new PropertyChangedEventArgs("Pressure")); } }
        public long Cloudness {
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
        public long Humidity
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
        public double Wind
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
        public long Temperature { get { return temperature; } set { temperature = value; PropertyChanged(this, new PropertyChangedEventArgs("Temperature")); } }
        public string Description { get { return description; } set { description = value; PropertyChanged(this, new PropertyChangedEventArgs("Description")); } }
        public string SearchInput { get { return searchInput; } set { searchInput = value; PropertyChanged(this, new PropertyChangedEventArgs("SearchInput")); } }
        

        public ICommand BtnFetchData { get; set; }
        public ICommand BtnSearch { get; set; }

        public MainViewModel()
        {
            
            BtnSearch = new Command(SearchLocation);
            _ = CurrentLocation();
        }

        //kalvin constant
        private const double kelvin = 273.15;
        public async void LoadData(double latitude, double longitude)
        {
            
            FetchData data = new FetchData(latitude, longitude);
            var json = await data.GetWeatherData();
            if(json != null)
            {
                var output = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(json);
                Temperature = ((long)(output.Current.Temp - kelvin));
                Humidity = output.Current.Humidity;
                Cloudness = output.Current.Clouds;
                Pressure = output.Current.Pressure;
                Wind = output.Current.WindSpeed;
                Icon = $"https://openweathermap.org/img/wn/{output.Current.Weather[0].Icon}@4x.png";

                var dt_sunset = GetDateFromeTimeStamp((double)output.Current.Sunset);
                var dt_sunrise = GetDateFromeTimeStamp((double)output.Current.Sunrise);
                Sunset = $"Sunset: {dt_sunset:HH:mm tt}";
                Sunrise = $"Sunrise: {dt_sunrise:HH:mm tt}";

                Description = output.Current.Weather[0].Description;


                daily.Clear();
                foreach (var item in output.Daily)
                {
                    var dateTime = GetDateFromeTimeStamp(item.Dt);
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
                    var dateTime = GetDateFromeTimeStamp(item.Dt);
                    item.Time = $"{dateTime:HH:mm tt}";
                    item.Temp = ((long)(item.Temp - kelvin));
                    hourly.Add(item);
                    Console.WriteLine("weeee " + dateTime.TimeOfDay);
                }
            }

        }
        private DateTime GetDateFromeTimeStamp(double time_stamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(time_stamp).ToLocalTime();
            return dt;
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
                try
                {
                    var results = await Geocoding.GetLocationsAsync(SearchInput);
                    var place = results.FirstOrDefault();
                    if (place != null)
                    {
                        Location = searchInput;
                        LoadData(place.Latitude, place.Longitude);
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    //not supported on device exception
                    Console.WriteLine(fnsEx.Message);
                    await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong", "Cancel");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    // Handle not enabled on device exception
                    await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong", "Cancel");
                    Console.WriteLine(fneEx.Message);
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                    await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong", "Cancel");
                    Console.WriteLine(pEx.Message);
                }
                catch (Exception ex)
                {
                    // Unable to get location
                    await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong", "Cancel");
                    Console.WriteLine(ex.Message);
                }

            }
        }

    }
}
