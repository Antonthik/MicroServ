using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MicroServ
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;
    
        // Счётчик для метрики CPU
        private PerformanceCounter _hddCounter;
    
    
        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        }
    
        public Task Execute(IJobExecutionContext context)
        {    
            // Получаем значение занятости CPU           
            var hddUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            //var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var time = unixTime;
            // Теперь можно записать что-то посредством репозитория

            _repository.Create(new HddMetric { Time = time, Value = hddUsageInPercents });
    
            return Task.CompletedTask;
        }
    }
    
    //public class HddMetricJob : IJob
    //{
    //    private IHddMetricsRepository _repository;
    //
    //    public HddMetricJob(IHddMetricsRepository repository)
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
