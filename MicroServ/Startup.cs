using AutoMapper;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;



namespace MicroServ
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;";


        //Первый вариант
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddSingleton<WeatherHolder>();//добавляем 
            services.AddSingleton<AgentInfo>();
        
            //Паттерн Repository
            ConfigureSqlLiteConnection(services); //Паттерн Repository
            //services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>(); //Паттерн Repository
            services.AddScoped<CpuMetricsRepository>(); //Паттерн Repository
            services.AddScoped<HddMetricsRepository>(); //Паттерн Repository
            services.AddScoped<RamMetricsRepository>(); //Паттерн Repository
            services.AddScoped<NetMetricsRepository>(); //Паттерн Repository



            //Планировщик заданий
            services.AddSingleton<IJobFactory, SingletonJobFactory>(); //Планировщик
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//Хранение заданий
            services.AddSingleton<ICpuMetricsRepository,CpuMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
            services.AddSingleton<INetMetricsRepository, NetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();

            // Добавляем нашу задачу
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton<NetMetricJob>();
            services.AddSingleton<HddMetricJob>();

            services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob),cronExpression: "0/5 * * * * ?")); // Запускать каждые 5 секунд
            services.AddSingleton(new JobSchedule(jobType: typeof(RamMetricJob),cronExpression: "0/5 * * * * ?"));
            services.AddSingleton(new JobSchedule(jobType: typeof(NetMetricJob), cronExpression: "0/5 * * * * ?"));
            services.AddSingleton(new JobSchedule(jobType: typeof(HddMetricJob), cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();

        }


        //Второй вариант
        // Этот метод вызывается средой выполнения. Используем его для добавления служб в контейнер
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddScoped<CpuMetricsRepository>(); //Паттерн Repository
        //    services.AddScoped<HddMetricsRepository>(); //Паттерн Repository
        //    services.AddScoped<RamMetricsRepository>(); //Паттерн Repository
        //    services.AddScoped<NetMetricsRepository>(); //Паттерн Repository
        //
        //
        //    //var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
        //    //var mapper = mapperConfiguration.CreateMapper();
        //    //
        //    //services.AddSingleton(mapper);
        //
        //    services.AddFluentMigratorCore()
        //        .ConfigureRunner(rb => rb
        //            // Добавляем поддержку SQLite 
        //            .AddSQLite()
        //            // Устанавливаем строку подключения
        //            .WithGlobalConnectionString(ConnectionString)
        //            // Подсказываем, где искать классы с миграциями
        //            .ScanIn(typeof(Startup).Assembly).For.Migrations()
        //        ).AddLogging(lb => lb
        //            .AddFluentMigratorConsole());
        //}

        //Паттерн Repository
        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
        }
        
        //Паттерн Repository
        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();        
                command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,value INT, time LONG)";
                command.ExecuteNonQuery();
            }
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY,value INT, time LONG)";
                command.ExecuteNonQuery();
            }
        
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DROP TABLE IF EXISTS rammetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY,value INT, time LONG)";
                command.ExecuteNonQuery();
            }
        
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DROP TABLE IF EXISTS netmetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE netmetrics(id INTEGER PRIMARY KEY,value INT, time LONG)";
                command.ExecuteNonQuery();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();              
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Запускаем миграции
            //migrationRunner.MigrateUp();
        }





    }
}
