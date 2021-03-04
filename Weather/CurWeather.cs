using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Net.Http;

namespace Weather
{
    public class CurWeather
    {
        public string Name { get; set; }        

        public double dt { get; set; }

        public Main Main { get; set; }

        public Clouds Clouds { get; set; }

        public Wind Wind { get; set; }    

    }

    public class Forecast
    {
        public City City { get; set; }
        
        public List<CurWeather> List { get; set; }

    }

    public class Main
    {
        public double Temp { get; set; }

        public double Temp_max { get; set; }

        public double Temp_min { get; set; }       

    }

    public class Clouds
    {
        public int All { get; set; }

    }

    public class Wind
    {
        public double Speed { get; set; }

    }

    public class City
    {
        public string Name { get; set; }

    }

    public class Forecast_weather
    {
        public string Date { get; set; }

        public string City { get; set; }

        public double Temp_min { get; set; }

        public double Temp_max { get; set; }

        public double Wind_speed { get; set; }

        public int Clouds { get; set; }      

    }
}
