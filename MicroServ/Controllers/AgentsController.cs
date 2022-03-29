using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// Контролер для менеджера метрик,который управляет агентами метрик
/// </summary>
namespace MicroServ.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public AgentsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Регистрация 
        /// </summary>
        /// <param name="agentInfo"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            return Ok();
        }

        /// <summary>
        /// Включение агента метрик
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        /// <summary>
        /// Выключение агента метрик
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            return Ok();
        }

        ////урок6 
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute]long fromTime, [FromRoute] long toTime)
        {
            var NameMetrics= new List<string> {"","cpu","hdd","net","ram"};

            string Uri = $"http://localhost:5000/api/metrics/{NameMetrics[agentId]}/allfromto/from/{fromTime}/to/{toTime}";
            var request = new HttpRequestMessage(HttpMethod.Get, Uri);
            //request.Headers.Add("Accept", "application/vnd.github.v3+json");
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                 var responseStream = response.Content.ReadAsStringAsync().Result;
                 //var metricsResponse = JsonSerializer.Deserialize<AllCpuMetricsResponse>(responseStream);
                JObject jObject = JObject.Parse(responseStream);
                JToken list = jObject["metrics"];
                List<CpuMetricDto> metrics = list.ToObject<List<CpuMetricDto>>();
                return Ok(metrics);

                //var responseStream = response.Content.ReadAsStreamAsync().Result;
                //var metricsResponse = JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream).Result;
            }
            else
            {
                // ошибка при получении ответа
            }
            return Ok();
        }

    }
}
