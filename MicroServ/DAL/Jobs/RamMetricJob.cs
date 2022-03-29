using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MicroServ
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;
    
        // Счётчик для метрики CPU
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }


    
        public Task Execute(IJobExecutionContext context)
        {
    
            // Получаем значение занятости CPU           
            var ramUsageInPercents = Convert.ToInt32(_ramCounter.NextValue());


            // Узнаем, когда мы сняли значение метрики
            //var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var time = unixTime;
            // Теперь можно записать что-то посредством репозитория

            _repository.Create(new RamMetric { Time = time, Value = ramUsageInPercents });
    
            return Task.CompletedTask;
        }
    }
    
    //public class RamMetricJob : IJob
    //{
    //    private IRamMetricsRepository _repository;
    //
    //    public RamMetricJob(IRamMetricsRepository repository)
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
