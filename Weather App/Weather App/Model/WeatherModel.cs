using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Weather_App
{

    public partial class WeatherModel
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Timezone { get; set; }
        public long TimezoneOffset { get; set; }
        public Current Current { get; set; }
        public List<Current> Hourly { get; set; }
        public List<Daily> Daily { get; set; }

    }

    public partial class Current
    {
        public long Dt { get; set; }
        public long? Sunrise { get; set; }
        public long? Sunset { get; set; }
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public long Pressure { get; set; }
        public long Humidity { get; set; }
        public double DewPoint { get; set; }
        public double Uvi { get; set; }
        public long Clouds { get; set; }
        public long Visibility { get; set; }
        [JsonProperty("wind_speed")]
        public double WindSpeed { get; set; }
        public long WindDeg { get; set; }
        public double WindGust { get; set; }
        public List<Weather> Weather { get; set; }
        public long? Pop { get; set; }
        public string Time { get; set; }


    }

    public partial class Weather
    {
        public long Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@4x.png";
    }

    public partial class Daily
    {
        public long Dt { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
        public long Moonrise { get; set; }
        public long Moonset { get; set; }
        public double MoonPhase { get; set; }
        public Temp Temp { get; set; }
        public FeelsLike FeelsLike { get; set; }
        public long Pressure { get; set; }
        public long Humidity { get; set; }
        public double DewPoint { get; set; }
        public double WindSpeed { get; set; }
        public long WindDeg { get; set; }
        public double WindGust { get; set; }
        public List<Weather> Weather { get; set; }
        public long Clouds { get; set; }
        public long Pop { get; set; }
        public double Uvi { get; set; }
        public DayOfWeek Day { get; internal set; }
    }

    public partial class FeelsLike
    {
        public double Day { get; set; }
        public double Night { get; set; }
        public double Eve { get; set; }
        public double Morn { get; set; }
    }

    public partial class Temp
    {
        public double Day { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Night { get; set; }
        public double Eve { get; set; }
        public double Morn { get; set; }
    }

    public enum Description { BrokenClouds, ClearSky, FewClouds, OvercastClouds, ScatteredClouds };

    public enum Main { Clear, Clouds };
}
