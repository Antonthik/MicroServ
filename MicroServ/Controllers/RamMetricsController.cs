﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
/// Контролер для агента метрики ram
/// </summary>
namespace MicroServ.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private RamMetricsRepository _repository;
        public RamMetricsController(ILogger<RamMetricsController> logger, RamMetricsRepository repository)
        {
            _logger = logger;//элемент логирования
            _logger.LogDebug(1, "NLog встроен в RamMetricsController");//элемент логирования

            _repository = repository;
        }

        [HttpGet("allfromto/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int fromTime, [FromRoute] int toTime)
        {
            // Задаём конфигурацию для маппера. Первый обобщённый параметр — тип объекта источника, второй — тип объекта, в который перетекут данные из источника
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RamMetric, RamMetricDto>());
            var mapper = config.CreateMapper();
            //IList<RamMetric> metrics = _repository.GetFromTo(fromTime, toTime);
            IList<RamMetric> metrics = _repository.GetFromToAgent(fromTime, toTime);
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // Добавляем объекты в ответ, используя маппер
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }
        [HttpGet("/available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _repository.Create(new RamMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            _logger.LogInformation($"Добавлены данные для Ram метод Create: Value {request.Value} Time {request.Time}");//элемент логирования

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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RamMetric, RamMetricDto>());
            var mapper = config.CreateMapper();
            IList<RamMetric> metrics = _repository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                // Добавляем объекты в ответ, используя маппер
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }

        //[HttpGet("all")]
        //public IActionResult GetAll()
        //{
        //    var metrics = _repository.GetAll();
        //
        //    var response = new AllRamMetricsResponse()
        //    {
        //        Metrics = new List<RamMetricDto>()
        //    };
        //
        //    foreach (var metric in metrics)
        //    {
        //        response.Metrics.Add(new RamMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
        //    }
        //
        //    return Ok(response);
        //}
        //


    }
}
