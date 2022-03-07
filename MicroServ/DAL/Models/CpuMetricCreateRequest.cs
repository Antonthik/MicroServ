using System;

namespace MicroServ.Controllers
{
    public class CpuMetricCreateRequest
    {
        //public TimeSpan Time { get; set; }
        public long Time { get; set; }
        public int Value { get; set; }
    }

}