using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MicroServ
{
    public class NetMetricJob : IJob
    {
        private INetMetricsRepository _repository;
    
        // Счётчик для метрики CPU
        private PerformanceCounter _netCounter;


        public NetMetricJob(INetMetricsRepository repository)
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            string[] instancename = category.GetInstanceNames();//Определяем список сетевых устройств

            _repository = repository;
            _netCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instancename[1]);
        }
    
        public Task Execute(IJobExecutionContext context)
        {
    
            // Получаем значение занятости CPU           
            var netUsageInPercents = Convert.ToInt32(_netCounter.NextValue());


            // Узнаем, когда мы сняли значение метрики
            //var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var time = unixTime;
            // Теперь можно записать что-то посредством репозитория

            _repository.Create(new NetMetric { Time = time, Value = netUsageInPercents });
    
            return Task.CompletedTask;
        }
    }
    
    //public class NetMetricJob : IJob
    //{
    //    private INetMetricsRepository _repository;
    //
    //    public NetMetricJob(INetMetricsRepository repository)
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
