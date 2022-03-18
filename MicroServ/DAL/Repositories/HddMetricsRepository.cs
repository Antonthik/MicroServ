using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MicroServ
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        // Строка подключения
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор
        public HddMetricsRepository()
        {
            // Добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(HddMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)",
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
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM hddmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(HddMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        //time = item.Time.TotalSeconds,
                        time = item.Time,
                        id = item.Id
                    });
            }
        }

        public IList<HddMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // Читаем, используя Query, и в шаблон подставляем тип данных,
                // объект которого Dapper, он сам заполнит его поля
                // в соответствии с названиями колонок
                return connection.Query<HddMetric>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            }
        }

        public HddMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<HddMetric>("SELECT Id, Time, Value FROM hddmetrics WHERE id=@id",
                    new { id = id });
            }
        }
    }

    // public class HddMetricsRepository : IHddMetricsRepository
    //{
    //    private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
    //    // Инжектируем соединение с базой данных в наш репозиторий через конструктор
    //
    //    public void Create(HddMetric item)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();
    //        // Создаём команду
    //        using var cmd = new SQLiteCommand(connection);
    //        // Прописываем в команду SQL-запрос на вставку данных
    //        cmd.CommandText = "INSERT INTO hddmetrics(value, time) VALUES(@value, @time)";
    //
    //        // Добавляем параметры в запрос из нашего объекта
    //        cmd.Parameters.AddWithValue("@value", item.Value);
    //
    //        // В таблице будем хранить время в секундах, поэтому преобразуем перед записью в секунды
    //        // через свойство
    //         //cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
    //           cmd.Parameters.AddWithValue("@time", item.Time);
    //        // подготовка команды к выполнению
    //        cmd.Prepare();
    //
    //        // Выполнение команды
    //        cmd.ExecuteNonQuery();
    //    }
    //
    //    public void Delete(int id)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();
    //        using var cmd = new SQLiteCommand(connection);
    //        // Прописываем в команду SQL-запрос на удаление данных
    //        cmd.CommandText = "DELETE FROM hddmetrics WHERE id=@id";
    //
    //        cmd.Parameters.AddWithValue("@id", id);
    //        cmd.Prepare();
    //        cmd.ExecuteNonQuery();
    //    }
    //
    //    public void Update(HddMetric item)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        using var cmd = new SQLiteCommand(connection);
    //        // Прописываем в команду SQL-запрос на обновление данных
    //        cmd.CommandText = "UPDATE hddmetrics SET value = @value, time = @time WHERE id=@id;";
    //        cmd.Parameters.AddWithValue("@id", item.Id);
    //        cmd.Parameters.AddWithValue("@value", item.Value);
    //        //cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
    //        cmd.Parameters.AddWithValue("@time", item.Time);
    //        cmd.Prepare();
    //
    //        cmd.ExecuteNonQuery();
    //    }
    //
    //    public IList<HddMetric> GetAll()
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();
    //        using var cmd = new SQLiteCommand(connection);
    //
    //        // Прописываем в команду SQL-запрос на получение всех данных из таблицы
    //        cmd.CommandText = "SELECT * FROM hddmetrics";
    //
    //        var returnList = new List<HddMetric>();
    //
    //        using (SQLiteDataReader reader = cmd.ExecuteReader())
    //        {
    //            // Пока есть что читать — читаем
    //            while (reader.Read())
    //            {
    //                // Добавляем объект в список возврата
    //                returnList.Add(new HddMetric
    //                {
    //                    Id = reader.GetInt32(0),
    //                    Value = reader.GetInt32(1),
    //                    // Налету преобразуем прочитанные секунды в метку времени
    //                    //Time = TimeSpan.FromSeconds(reader.GetInt32(2))
    //                    Time = reader.GetInt32(2)
    //                });
    //            }
    //        }
    //
    //        return returnList;
    //    }
    //
    //    public HddMetric GetById(int id)
    //    {
    //        using var connection = new SQLiteConnection(ConnectionString);
    //        connection.Open();
    //        using var cmd = new SQLiteCommand(connection);
    //        cmd.CommandText = "SELECT * FROM hddmetrics WHERE id=@id";
    //        using (SQLiteDataReader reader = cmd.ExecuteReader())
    //        {
    //            // Если удалось что-то прочитать
    //            if (reader.Read())
    //            {
    //                // возвращаем прочитанное
    //                return new HddMetric
    //                {
    //                    Id = reader.GetInt32(0),
    //                    Value = reader.GetInt32(1),
    //                    //Time = TimeSpan.FromSeconds(reader.GetInt32(1))
    //                };
    //            }
    //            else
    //            {
    //                // Не нашлась запись по идентификатору, не делаем ничего
    //                return null;
    //            }
    //        }
    //    }
    //}
}