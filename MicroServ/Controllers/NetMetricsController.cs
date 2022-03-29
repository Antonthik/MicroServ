using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// Контролер для агента метрики net
/// </summary>
namespace MicroServ.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/metrics/net")]
    [ApiController]
    public class NetMetricsController : ControllerBase
    {
        private readonly ILogger<NetMetricsController> _logger;
        private NetMetricsRepository _repository;
        public NetMetricsController(ILogger<NetMetricsController> logger, NetMetricsRepository repository)
        {
            _logger = logger;//элемент логирования
            _logger.LogDebug(1, "NLog встроен в NetMetricsController");//элемент логирования

            _repository = repository;
        }

        [HttpGet("allfromto/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int fromTime, [FromRoute] int toTime)
        {
            // Задаём конфигурацию для маппера. Первый обобщённый параметр — тип объекта источника, второй — тип объекта, в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetMetric, NetMetricDto>());
            var mapper = config.CreateMapper();
            //IList<NetMetric> metrics = _repository.GetFromTo(fromTime, toTime);
            IList<NetMetric> metrics = _repository.GetFromToAgent(fromTime, toTime);
            var response = new AllNetMetricsResponse()
            {
                Metrics = new List<NetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // Добавляем объекты в ответ, используя маппер
                response.Metrics.Add(mapper.Map<NetMetricDto>(metric));
            }
            return Ok(response);
        }
        [HttpGet("/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] NetMetricCreateRequest request)
        {
            _repository.Create(new NetMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            _logger.LogInformation($"Добавлены данные для Net метод Create: Value {request.Value} Time {request.Time}");//элемент логирования

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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetMetric, NetMetricDto>());
            var mapper = config.CreateMapper();
            IList<NetMetric> metrics = _repository.GetAll();
            var response = new AllNetMetricsResponse()
            {
                Metrics = new List<NetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // Добавляем объекты в ответ, используя маппер
                response.Metrics.Add(mapper.Map<NetMetricDto>(metric));
            }
            return Ok(response);
        }



        //[HttpGet("all")]
        //public IActionResult GetAll()
        //{
        //    var metrics = _repository.GetAll();
        //
        //    var response = new AllNetMetricsResponse()
        //    {
        //        Metrics = new List<NetMetricDto>()
        //    };
        //
        //    foreach (var metric in metrics)
        //    {
        //        response.Metrics.Add(new NetMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
        //    }
        //
        //    return Ok(response);
        //}
        //
    }
}
