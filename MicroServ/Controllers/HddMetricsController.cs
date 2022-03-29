using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// Контролер для агента метрики hdd
/// </summary>
namespace MicroServ.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private HddMetricsRepository _repository;
        public HddMetricsController(ILogger<HddMetricsController> logger, HddMetricsRepository repository)
        {
            _logger = logger;//элемент логирования
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");//элемент логирования

            _repository = repository;
        }
        
        [HttpGet("allfromto/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int fromTime, [FromRoute] int toTime)
        {
            // Задаём конфигурацию для маппера. Первый обобщённый параметр — тип объекта источника, второй — тип объекта, в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<HddMetric, HddMetricDto>());
            var mapper = config.CreateMapper();
            //IList<HddMetric> metrics = _repository.GetFromTo(fromTime, toTime);
            IList<HddMetric> metrics = _repository.GetFromToAgent(fromTime, toTime);
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // Добавляем объекты в ответ, используя маппер
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            _repository.Create(new HddMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            _logger.LogInformation($"Добавлены данные для Hdd метод Create: Value {request.Value} Time {request.Time}");//элемент логирования

            return Ok();
        }

        /// <summary>
        /// Используем Mapper
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            // Задаём конфигурацию для маппера. Первый обобщённый параметр — тип объекта источника, второй — тип объекта, в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<HddMetric, HddMetricDto>());
            var mapper = config.CreateMapper();
            IList<HddMetric> metrics = _repository.GetAll();
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // Добавляем объекты в ответ, используя маппер
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }
            return Ok(response);
        }



        //[HttpGet("all")]
        //public IActionResult GetAll()
        //{
        //    var metrics = _repository.GetAll();
        //
        //    var response = new AllHddMetricsResponse()
        //    {
        //        Metrics = new List<HddMetricDto>()
        //    };
        //
        //    foreach (var metric in metrics)
        //    {
        //        response.Metrics.Add(new HddMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
        //    }
        //
        //    return Ok(response);
        //}
        //

    }
}
