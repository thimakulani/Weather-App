﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather_App.Service;
using Xamarin.Forms;

namespace Weather_App.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private WeatherModel weather;


        private ObservableCollection<Daily> weatherList = new ObservableCollection<Daily>();
        public ObservableCollection<Daily> WeatherList { get { return weatherList; } set { weatherList = value; } }
        private long temperature;
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
        public long Temperature { get { return temperature; } set { temperature = value; PropertyChanged(this, new PropertyChangedEventArgs("Temperature")); } }
        public string Description { get { return description; } set { description = value; PropertyChanged(this, new PropertyChangedEventArgs("Description")); } }
        public string SearchInput { get { return searchInput; } set { searchInput = value; PropertyChanged(this, new PropertyChangedEventArgs("SearchInput")); } }
        

        public ICommand RefreshCmd { get; set; }
        public ICommand BtnFetchData { get; set; }
        public ICommand BtnSearch { get; set; }

        public MainViewModel()
        {
            RefreshCmd = new Command(RefreshData);
            BtnSearch = new Command(SearchLocation);
            BtnFetchData = new Command(BtnFetchInfo);
            LoadData();
        }

        private void BtnFetchInfo(object obj)
        {
            LoadData();
        }

        public async void LoadData()
        {






            FetchData data = new FetchData("", 2, 2);
            var json = await data.WeatherData2();
            Console.WriteLine(json);

            if(json != null)
            {
                var output = WeatherModel.FromJson(json);

                Temperature = 19;
                //icon = $"https://openweathermap.org/img/wn/{output.Current.}.png";
                Humidity = $"{output.Current.Humidity}";
                Cloudness = $"{output.Current.Clouds}";
                Pressure = $"{output.Current.Pressure}";
                Wind = $"{output.Current.WindSpeed}";
                Icon = $"https://openweathermap.org/img/wn/{output.Current.Weather[0].Icon}@4x.png";
                Description = $"{output.Current.Weather[0].Main}";
                DescriptionConverter descriptionConverter = new DescriptionConverter();
                
                //Location = DateTimeOffset.Parse(output.TimezoneOffset.ToString()).Date.ToString("dd, MMM");
                _ =CurrentLocation();

                foreach (var item in output.Daily)
                {
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(item.Dt).ToLocalTime();
                    item.Day = dateTime.DayOfWeek;
                    if (DateTime.Now.Date != dateTime.Date)
                    {
                        item.Temp.Min = ((long)(item.Temp.Min - 273.15));
                        weatherList.Add(item);

                        

                        //temperature = ((long)((item.Temp.Min - 273.15) * (9 / 5) + 32));
                    }
                }
            }
            //if (json != null)
            //{
            //    weather = WeatherModel.FromJson(json);
            //    location = $"{weather.City.Name}, {weather.City.Country}";
            //    temperature = ((long)((weather.List[0].Deg - 273.15) * (9 / 5) + 32));
            //    icon = $"https://openweathermap.org/img/wn/{weather.List[0].Weather[0].Icon}.png";
            //    description = weather.List[0].Weather[0].Description;



            //    foreach (var item in weather.List)
            //    {
            //        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //        dateTime = dateTime.AddSeconds(item.Dt).ToLocalTime();
            //        item.Day = dateTime.DayOfWeek;
            //        if(DateTime.Now.Date == dateTime.Date)
            //        {
            //            Console.WriteLine("match");
            //        }
            //        item.Deg = ((long)((item.Deg - 273.15) * (9 / 5) + 32));
            //        // DateTime dateTime = new DateTime();



            //        Console.WriteLine("dfgh" + item.Day);

            //        weatherList.Add(item);
            //    }
            //}
        }
       private async Task CurrentLocation()
        {
            var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

            var result = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(location);
            var addr = result.FirstOrDefault();
            Location = $"{addr.Locality}, {addr.CountryCode}";
            SearchInput = addr.Locality; 
        }
        private void SearchLocation(object obj)
        {

        }

        private async void RefreshData(object obj)
        {
            
        }
    }
}