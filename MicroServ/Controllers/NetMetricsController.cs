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
        /// <summary>
        /// Агент сбора метрики dotnet
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <returns></returns>
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



        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll();

            var response = new AllNetMetricsResponse()
            {
                Metrics = new List<NetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetMetricDto { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }

    }
}
