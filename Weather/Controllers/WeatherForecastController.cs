using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;



namespace Weather.Controllers
{
    [ApiController]
    [Route("api/")]
   
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        /// <response code="200">Success: getting current weather</response>
        /// <response code="400">Fail: wrong city name</response>   
        [HttpGet("GetCurrentWeather")]
        public async Task<IActionResult> Cur_weather(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=b8d8475d4cbc4e479aede93a43f8182d&units=metric");
                    response.EnsureSuccessStatusCode();

                    var dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<CurWeather>(stringResult);
                    return Ok(new
                    {
                        Date = dateTime.AddSeconds(rawWeather.dt).ToString("yyyy-MM-dd"),
                        City = rawWeather.Name,
                        Temp = rawWeather.Main.Temp,
                        Wind_speed = rawWeather.Wind.Speed,                        
                        Clouds = rawWeather.Clouds.All                        
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }

        /// <response code="200">Success: getting forecast for 5 days</response>
        /// <response code="400">Fail: wrong city name</response>
        [HttpGet("GetForecast")]
        public async Task<IActionResult> Forecast(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/forecast?q={city}&appid=b8d8475d4cbc4e479aede93a43f8182d&units=metric");
                    response.EnsureSuccessStatusCode();
                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<Forecast>(stringResult);
                    var dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                    return Ok(Enumerable.Range(0, 5).Select(index => new Forecast_weather
                    {
                        Date = dateTime.AddSeconds(rawWeather.List[index * 8].dt).ToString("yyyy-MM-dd"),
                        City = rawWeather.City.Name,
                        Temp_min = rawWeather.List[index * 8].Main.Temp_min,
                        Temp_max = rawWeather.List[index * 8].Main.Temp_max,
                        Wind_speed = rawWeather.List[index * 8].Wind.Speed,
                        Clouds = rawWeather.List[index * 8].Clouds.All
                    })
                    .ToArray());                
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }

}
