using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace MicroServ
{
    public class MetricsAgentClientsJob : IJob
    {
        //private MetricsAgentClient _agentclient;

        // Счётчик для метрики CPU
        // private PerformanceCounter _cpuCounter;
        private readonly HttpClient _httpClient;

        public MetricsAgentClientsJob(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_agentclient = agentclient;
            //_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //_agentclient = new HttpClient();
        }
    
        public Task Execute(IJobExecutionContext context)
        {

            // var fff =_agentclient.GetAllHddMetricsAsync();

            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            long fromParameter = 1;
            long toParameter = unixTime;

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:5000/api/hddmetrics/allfromto/from/{fromParameter}/to/{toParameter}")
            };


            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                // return JsonSerializer.DeserializeAsync<AllHddMetricsApiResponse>(responseStream).Result;
                //return null;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
            }

           // return null;

            return Task.CompletedTask;
        }
    }
    
    //public class CpuMetricJob : IJob
    //{
    //    private ICpuMetricsRepository _repository;
    //
    //    public CpuMetricJob(ICpuMetricsRepository repository)
    //     {
    //         _repository = repository;
    //     }
    // 
    //     public Task Execute(IJobExecutionContext context)
    //     {
    //         // Теперь можно записать что-то посредством репозитория
    // 
    //         return Task.CompletedTask;
    //     }
    // }

}
