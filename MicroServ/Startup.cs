using AutoMapper;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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


        //������ �������
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddSingleton<WeatherHolder>();//��������� 
            services.AddSingleton<AgentInfo>();
            services.AddHttpClient();//����6 ��������� http ������

            //������� Repository
            ConfigureSqlLiteConnection(services); //������� Repository
            //services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>(); //������� Repository
            services.AddScoped<CpuMetricsRepository>(); //������� Repository
            services.AddScoped<HddMetricsRepository>(); //������� Repository
            services.AddScoped<RamMetricsRepository>(); //������� Repository
            services.AddScoped<NetMetricsRepository>(); //������� Repository

            //���� 7 Swagger
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API ������� ������ ����� ������",
                    Description = "����� ����� �������� � api ������ �������",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Semenov",
                        //Email = sa@mail.ru,
                        //Url = new Uri("https://kremlin.ru"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "����� �������, ��� ����� ��������� �� ������������",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });




            //����������� �������
            services.AddSingleton<IJobFactory, SingletonJobFactory>(); //�����������
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//�������� �������
            services.AddSingleton<ICpuMetricsRepository,CpuMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
            services.AddSingleton<INetMetricsRepository, NetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();

            //services.AddSingleton<MetricsAgentClient>();

            // ��������� ���� ������
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton<NetMetricJob>();
            services.AddSingleton<HddMetricJob>();

            //services.AddSingleton<MetricsAgentClientsJob>();

            services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob),cronExpression: "0/5 * * * * ?")); // ��������� ������ 5 ������
            services.AddSingleton(new JobSchedule(jobType: typeof(RamMetricJob),cronExpression: "0/5 * * * * ?"));
            services.AddSingleton(new JobSchedule(jobType: typeof(NetMetricJob), cronExpression: "0/5 * * * * ?"));
            services.AddSingleton(new JobSchedule(jobType: typeof(HddMetricJob), cronExpression: "0/5 * * * * ?"));

            //services.AddSingleton(new JobSchedule(jobType: typeof(MetricsAgentClientsJob), cronExpression: "0/5 * * * * ?"));
            

            services.AddHostedService<QuartzHostedService>();

        }


        //������ �������
        // ���� ����� ���������� ������ ����������. ���������� ��� ��� ���������� ����� � ���������
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddScoped<CpuMetricsRepository>(); //������� Repository
        //    services.AddScoped<HddMetricsRepository>(); //������� Repository
        //    services.AddScoped<RamMetricsRepository>(); //������� Repository
        //    services.AddScoped<NetMetricsRepository>(); //������� Repository
        //
        //
        //    //var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
        //    //var mapper = mapperConfiguration.CreateMapper();
        //    //
        //    //services.AddSingleton(mapper);
        //
        //    services.AddFluentMigratorCore()
        //        .ConfigureRunner(rb => rb
        //            // ��������� ��������� SQLite 
        //            .AddSQLite()
        //            // ������������� ������ �����������
        //            .WithGlobalConnectionString(ConnectionString)
        //            // ������������, ��� ������ ������ � ����������
        //            .ScanIn(typeof(Startup).Assembly).For.Migrations()
        //        ).AddLogging(lb => lb
        //            .AddFluentMigratorConsole());
        //}

        //������� Repository
        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
        }
        
        //������� Repository
        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                // ����� ����� ����� ������� ��� ����������
                // ������� ������� � ���������, ���� ��� ���� � ���� ������
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                // ���������� ������ � ���� ������
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
            //������� ������� ������
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DROP TABLE IF EXISTS cpumetricsagent";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE cpumetricsagent(id INTEGER PRIMARY KEY,value INT, time LONG,agentId INTEGER )";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS hddmetricsagent";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE hddmetricsagent(id INTEGER PRIMARY KEY,value INT, time LONG,agentId INTEGER )";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS rammetricsagent";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE rammetricsagent(id INTEGER PRIMARY KEY,value INT, time LONG,agentId INTEGER )";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS netmetricsagent";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE netmetricsagent(id INTEGER PRIMARY KEY,value INT, time LONG,agentId INTEGER )";
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
            // ��������� ��������
            //migrationRunner.MigrateUp();

            // ��������� middleware � �������� ��� ��������� Swagger-��������.
            app.UseSwagger();
            // ��������� middleware ��� ��������� swagger-ui
            // ��������� �������� Swagger JSON (���� ���������� �� ��������������� �������������,
            // �� ������� ����� �������� UI).
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ������� ������ ����� ������"); });
        }





    }
}
