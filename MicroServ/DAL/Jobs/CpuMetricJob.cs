using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MicroServ
{
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;
    
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;
    
    
        public CpuMetricJob(ICpuMetricsRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }
    
        public Task Execute(IJobExecutionContext context)
        {
    
            // Получаем значение занятости CPU           
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());


            // Узнаем, когда мы сняли значение метрики
            //var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var time = unixTime;
            // Теперь можно записать что-то посредством репозитория

            _repository.Create(new CpuMetric { Time = time, Value = cpuUsageInPercents });
    
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
