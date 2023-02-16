using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

namespace Protection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private static List<WeatherForecast> Data = new List<WeatherForecast>(); 

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPut, Route("forecasts/{forecastId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void UpdateForecast(int forecastId, [FromBody] WeatherForecast forecast)
        {
            var innerForecastIndx = Data.FindIndex(x => x.Id == forecastId);
            if (innerForecastIndx != -1)
            {
                Data[innerForecastIndx] = forecast;
            }
        }

        [HttpPost, Route("forecasts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void AddForecast([FromBody] WeatherForecast forecast)
        {
            Data.Add(forecast);
        }

        [HttpGet, Route("forecasts/{forecastId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
        public WeatherForecast Get(int forecastId)
        {
            return Data.Find(wf => wf.Id == forecastId);
        }

        [HttpGet, Route("forecasts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecast>))]
        public IEnumerable<WeatherForecast> Get()
        {
            return Data;
        }
    }
}