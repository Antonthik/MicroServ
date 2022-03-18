using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServ
{
    /// <summary>
    /// Планировщик
    /// Эта фабрика принимает IServiceProvider в конструкторе и реализует интерфейс IJobFactory.
    /// Важным методом считается NewJob(). Этим методом фабрика возвращает IJob, запрошенный планировщиком Quartz.
    /// В этой реализации мы напрямую делегируем IServiceProvider и позволяем контейнеру DI находить требуемый экземпляр.
    /// Приведение к IJob в конце обуславливается тем, что не универсальная версия GetRequiredService возвращает объект
    /// </summary>
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job) { }
    }

}
