using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MicroServ
{
    public class NetMetricsRepository : INetMetricsRepository
    {
        // Строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор
        public NetMetricsRepository()
        {
            // Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(NetMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO netmetrics(value, time) VALUES(@value, @time)",
                    // Анонимный объект с параметрами запроса
                    new
                    {
                        // Value подставится на место "@value" в строке запроса
                        // Значение запишется из поля Value объекта item
                        value = item.Value,

                        // Записываем в поле time количество секунд
                        // time = item.Time.TotalSeconds
                        time = item.Time
                    });
                connection.Execute("INSERT INTO netmetricsagent(value, time, agentId) VALUES(@value, @time, @agentId)",//добавляем в таблицу metricsagent
                    new
                    {
                        value = item.Value,
                        time = item.Time,
                        agentId = 3
                    });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM netmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(NetMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE netmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        //time = item.Time.TotalSeconds,
                        time = item.Time,
                        id = item.Id
                    });
            }
        }
        public IList<NetMetric> GetFromTo(long FromTime, long ToTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetMetric>("SELECT Id, Time, Value FROM netmetrics WHERE Time>=@FromTime and Time<=@ToTime",

                    new
                    {
                        FromTime = FromTime,
                        ToTime = ToTime
                    }).ToList();
            }
        }
        public IList<NetMetric> GetFromToAgent(long FromTime, long ToTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetMetric>("SELECT Id, Time, Value FROM netmetricsagent WHERE Time>=@FromTime and Time<=@ToTime",

                    new
                    {
                        FromTime = FromTime,
                        ToTime = ToTime
                    }).ToList();
            }
        }

        public IList<NetMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // Читаем, используя Query, и в шаблон подставляем тип данных,
                // объект которого Dapper, он сам заполнит его поля
                // в соответствии с названиями колонок
                return connection.Query<NetMetric>("SELECT Id, Time, Value FROM netmetrics").ToList();
            }
        }

        public NetMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<NetMetric>("SELECT Id, Time, Value FROM netmetrics WHERE id=@id",
                    new { id = id });
            }
        }
    }
    //
     //   
     //    public class NetMetricsRepository : INetMetricsRepository
     //   {
     //       private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
     //       // Инжектируем соединение с базой данных в наш репозиторий через конструктор
     //
     //       public void Create(NetMetric item)
     //       {
     //           using var connection = new SQLiteConnection(ConnectionString);
     //           connection.Open();
     //           // Создаём команду
     //           using var cmd = new SQLiteCommand(connection);
     //           // Прописываем в команду SQL-запрос на вставку данных
     //           cmd.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";
     //
     //           // Добавляем параметры в запрос из нашего объекта
     //           cmd.Paneteters.AddWithValue("@value", item.Value);
     //
     //           // В таблице будем хранить время в секундах, поэтому преобразуем перед записью в секунды
     //           // через свойство
     //            //cmd.Paneteters.AddWithValue("@time", item.Time.TotalSeconds);
     //              cmd.Paneteters.AddWithValue("@time", item.Time);
     //           // подготовка команды к выполнению
     //           cmd.Prepare();
     //
     //           // Выполнение команды
     //           cmd.ExecuteNonQuery();
     //       }
     //
     //       public void Delete(int id)
     //       {
     //           using var connection = new SQLiteConnection(ConnectionString);
     //           connection.Open();
     //           using var cmd = new SQLiteCommand(connection);
     //           // Прописываем в команду SQL-запрос на удаление данных
     //           cmd.CommandText = "DELETE FROM cpumetrics WHERE id=@id";
     //
     //           cmd.Paneteters.AddWithValue("@id", id);
     //           cmd.Prepare();
     //           cmd.ExecuteNonQuery();
     //       }
     //
     //       public void Update(NetMetric item)
     //       {
     //           using var connection = new SQLiteConnection(ConnectionString);
     //           using var cmd = new SQLiteCommand(connection);
     //           // Прописываем в команду SQL-запрос на обновление данных
     //           cmd.CommandText = "UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id;";
     //           cmd.Paneteters.AddWithValue("@id", item.Id);
     //           cmd.Paneteters.AddWithValue("@value", item.Value);
     //           //cmd.Paneteters.AddWithValue("@time", item.Time.TotalSeconds);
     //           cmd.Paneteters.AddWithValue("@time", item.Time);
     //           cmd.Prepare();
     //
     //           cmd.ExecuteNonQuery();
     //       }
     //
     //       public IList<NetMetric> GetAll()
     //       {
     //           using var connection = new SQLiteConnection(ConnectionString);
     //           connection.Open();
     //           using var cmd = new SQLiteCommand(connection);
     //
     //           // Прописываем в команду SQL-запрос на получение всех данных из таблицы
     //           cmd.CommandText = "SELECT * FROM cpumetrics";
     //
     //           var returnList = new List<NetMetric>();
     //
     //           using (SQLiteDataReader reader = cmd.ExecuteReader())
     //           {
     //               // Пока есть что читать — читаем
     //               while (reader.Read())
     //               {
     //                   // Добавляем объект в список возврата
     //                   returnList.Add(new NetMetric
     //                   {
     //                       Id = reader.GetInt32(0),
     //                       Value = reader.GetInt32(1),
     //                       // Налету преобразуем прочитанные секунды в метку времени
     //                       //Time = TimeSpan.FromSeconds(reader.GetInt32(2))
     //                       Time = reader.GetInt32(2)
     //                   });
     //               }
     //           }
     //
     //           return returnList;
     //       }
     //
     //       public NetMetric GetById(int id)
     //       {
     //           using var connection = new SQLiteConnection(ConnectionString);
     //           connection.Open();
     //           using var cmd = new SQLiteCommand(connection);
     //           cmd.CommandText = "SELECT * FROM cpumetrics WHERE id=@id";
     //           using (SQLiteDataReader reader = cmd.ExecuteReader())
     //           {
     //               // Если удалось что-то прочитать
     //               if (reader.Read())
     //               {
     //                   // возвращаем прочитанное
     //                   return new NetMetric
     //                   {
     //                       Id = reader.GetInt32(0),
     //                       Value = reader.GetInt32(1),
     //                       //Time = TimeSpan.FromSeconds(reader.GetInt32(1))
     //                   };
     //               }
     //               else
     //               {
     //                   // Не нашлась запись по идентификатору, не делаем ничего
     //                   return null;
     //               }
     //           }
     //       }
     //   }
}