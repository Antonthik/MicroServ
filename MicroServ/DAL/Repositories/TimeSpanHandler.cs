using Dapper;
using System;
using System.Data;

namespace MicroServ
{
    // Задаём хендлер для парсинга значений в TimeSpan, если таковые попадутся в наших классах моделей
    public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value)
            => TimeSpan.FromSeconds((long)value);

        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
            => parameter.Value = value;
    }

}