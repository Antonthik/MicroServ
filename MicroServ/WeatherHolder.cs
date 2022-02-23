using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServ
{
    /// <summary>
    /// Класс для хранения и изменения данных
    /// </summary>
    public class WeatherHolder
    {
       //public Dictionary<string, WeatherForecast> HolderDic = new Dictionary<string, WeatherForecast>();
       public Dictionary<string, WeatherForecast> Values { get; set; }

       /// <summary>
       /// Добавление данных
       /// </summary>
       /// <param name="d"></param>
       /// <param name="w"></param>
      public void Add(string d,WeatherForecast w)
        {
            if (Values == null)
            {
                Values = new Dictionary<string, WeatherForecast>();
                Values.Add(d, w);
            }
            else
            {
                Values.Add(d, w);
            }     
        }

        /// <summary>
        /// Чтение данных
        /// </summary>
        /// <returns></returns>
        public string[] Get()
        {
            List<string> list = new List<string>();
            if (Values != null)
            {
                foreach (var o in Values.Values)
                {
                    list.Add($"{o.Date} {o.TemperatureC} {o.TemperatureF} {o.Summary}");
                }
            }
            else
            {
                list.Add("Нет данных");
            }            
            return list.ToArray();
        }

        /// <summary>
        /// Удаление из словаря
        /// </summary>
        /// <param name="dateToDelete"></param>
        public void Delete(string dateToDelete)
        {
            if (Values != null) 
            {
               Values.Remove(dateToDelete);
            }            
        }
        /// <summary>
        /// Обновление строки
        /// </summary>
        /// <param name="stringsToUpdate"></param>
        /// <param name="newValue">ищем строку по дате и времени</param>
        public void Update(string stringsToUpdate, string newValue)
        {
            var parsedint = int.Parse(newValue);
            Values[stringsToUpdate].TemperatureC = parsedint;
        }

    }
}
