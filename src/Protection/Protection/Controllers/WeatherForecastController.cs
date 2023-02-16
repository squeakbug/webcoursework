using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System;

namespace Protection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<WeatherForecast> Data = new List<WeatherForecast>(); 

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger ?? throw new Exception("Null logger");
        }

        [HttpPut, Route("forecasts/{forecastId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateForecast(int forecastId, [FromBody] WeatherForecast forecast)
        {
            var innerForecastIndx = Data.FindIndex(x => x.Id == forecastId);
            if (innerForecastIndx != -1)
            {
                Data[innerForecastIndx] = forecast;
                return Ok(forecast);
            }
            else
            {
                return NotFound();
            }
        }

        private int GetNewListId(List<WeatherForecast> data)
        {
            if (data.Count == 0)
                return 0;
            else
            {
                int maxId = data[0].Id;
                foreach (var item in data)
                {
                    if (item.Id > maxId)
                    {
                        maxId = item.Id;
                    }
                }
                return maxId + 1;
            }
        }

        [HttpPost, Route("forecasts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void AddForecast([FromBody] WeatherForecast forecast)
        {
            forecast.Id = GetNewListId(Data);
            Data.Add(forecast);
        }

        [HttpGet, Route("forecasts/{forecastId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int forecastId)
        {
            var d = Data.Find(wf => wf.Id == forecastId);
            if (d == null)
                return Ok(d);
            else
                return NotFound();
        }

        [HttpGet, Route("forecasts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecast>))]
        public IEnumerable<WeatherForecast> Get()
        {
            return Data;
        }
    }
}