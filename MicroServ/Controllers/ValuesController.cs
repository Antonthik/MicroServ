using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServ.Controllers
{
    [Route("api/crud")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private WeatherHolder _holder;//объект для хранения данных
        public ValuesController(WeatherHolder holder)
        {
            _holder = holder;
        }

        [HttpGet, Route("read")]
        public IActionResult Get()
        {
            return Ok(_holder.Get());

        }

        /// <summary>
        /// Генерим данные
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public IActionResult Create()
        {
            var rng = new Random();
            var w = new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
            
            _holder.Add(w.Date.ToString("dd.MM.yyyy HH:mm:ss"), w);
            return Ok();
        }

        /// <summary>
        /// Удаляем запись
        /// </summary>
        /// <param name="dateToDelete - указываем дату-ключ "></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string dateToDelete)
        {
            //var parsedDate = DateTime.Parse(dateToDelete);
            //_holder.Holder = _holder.Values.Where(w => w != stringsToDelete).ToList();
            _holder.Delete(dateToDelete);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] string stringsToUpdate, [FromQuery] string newValue)
        {
            _holder.Update(stringsToUpdate, newValue);
            return Ok();
        }
    }
}
