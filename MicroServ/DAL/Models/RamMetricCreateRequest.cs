using System;

namespace MicroServ.Controllers
{
    public class RamMetricCreateRequest
    {
        //public TimeSpan Time { get; set; }
        public long Time { get; set; }
        public int Value { get; set; }
    }

}