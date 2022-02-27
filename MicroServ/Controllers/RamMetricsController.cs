﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/metrics/ram/available")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        /// <summary>
        /// Агент сбора метрики ram
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

    }
}
