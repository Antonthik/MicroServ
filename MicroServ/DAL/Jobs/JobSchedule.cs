using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServ
{
    /// <summary>
    /// DTO-класс, где будем хранить расписание запуска
    /// </summary>
    public class JobSchedule
    {
        public Type JobType { get; }
        public string CronExpression { get; }
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }
    }

}
