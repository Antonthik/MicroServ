using MicroServ;
using MicroServ.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricManadgerTest
{
    public class MetricsManagerUnitTests
    {
        public class CpuMetricsControllerUnitTests
        {
           // private readonly ILogger<CpuMetricsController>_logger;
            private CpuMetricsController controller;
            private Mock<CpuMetricsRepository> mock;
            private Mock<ILogger<CpuMetricsController>> mock2;
            public CpuMetricsControllerUnitTests()
            {
               //_logger = logger;
                mock2 = new Mock<ILogger<CpuMetricsController>>();
                mock = new Mock<CpuMetricsRepository>();
                controller = new CpuMetricsController(mock2.Object, mock.Object);
            }

            [Fact]
            public void Create_ShouldCall_Create_From_Repository()
            {
                // Устанавливаем параметр заглушки
                // В заглушке прописываем, что в репозиторий прилетит CpuMetric-объект
                mock.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

                // Выполняем действие на контроллере
                //var result = controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });
                var result = controller.Create(new CpuMetricCreateRequest { Time = 1, Value = 100 });

                // Проверяем заглушку на то, что пока работал контроллер
                // Вызвался метод Create репозитория с нужным типом объекта в параметре
                mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
            }
        }

        // /// <summary>
        // /// Test cpu
        // /// </summary>
        // public class CpuMetricsControllerUnitTests
        // {
        //     public CpuMetricsControllerUnitTests()
        //     {
        //         controller = new CpuMetricsController();//экземпляр контролера,который тестируем
        //     }
        //
        //     [Fact]
        //     public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
        //     {
        //         //Arrange - задаем входные параметры теста
        //         var agentId = 1;
        //         var fromTime = TimeSpan.FromSeconds(0);
        //         var toTime = TimeSpan.FromSeconds(100);
        //
        //         //Act - выполняем тест
        //         var result = controller.GetMetricsFromAgent(agentId, fromTime, toTime);
        //
        //         // Assert - сравнение ожидаемый результат с тестовым
        //         _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
        //     }
        // }
        //public class DotNetMetricsControllerUnitTests
        //{
        //    private DotNetMetricsController controller;
        //    public DotNetMetricsControllerUnitTests()
        //    {
        //        controller = new DotNetMetricsController();//экземпляр контролера,который тестируем
        //    }
        //
        //    [Fact]
        //    public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
        //    {
        //        //Arrange - задаем входные параметры теста
        //        //var agentId = 1;
        //        var fromTime = TimeSpan.FromSeconds(0);
        //        var toTime = TimeSpan.FromSeconds(100);
        //
        //        //Act - выполняем тест
        //        var result = controller.GetMetricsFromAgent(fromTime, toTime);
        //
        //        // Assert - сравнение ожидаемый результат с тестовым
        //        _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
        //    }
        //}
        //public class NetMetricsControllerUnitTests
        //{
        //    private NetMetricsController controller;
        //    public NetMetricsControllerUnitTests()
        //    {
        //        controller = new NetMetricsController();//экземпляр контролера,который тестируем
        //    }
        //
        //    [Fact]
        //    public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
        //    {
        //        //Arrange - задаем входные параметры теста
        //        //var agentId = 1;
        //        var fromTime = TimeSpan.FromSeconds(0);
        //        var toTime = TimeSpan.FromSeconds(100);
        //
        //        //Act - выполняем тест
        //        var result = controller.GetMetricsFromAgent(fromTime, toTime);
        //
        //        // Assert - сравнение ожидаемый результат с тестовым
        //        _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
        //    }
        //}
        //public class HddMetricsControllerUnitTests
        //{
        //    private HddMetricsController controller;
        //    public HddMetricsControllerUnitTests()
        //    {
        //        controller = new HddMetricsController();//экземпляр контролера,который тестируем
        //    }
        //
        //    [Fact]
        //    public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
        //    {
        //        //Arrange - задаем входные параметры теста
        //        //var agentId = 1;
        //        var fromTime = TimeSpan.FromSeconds(0);
        //        var toTime = TimeSpan.FromSeconds(100);
        //
        //        //Act - выполняем тест
        //        var result = controller.GetMetricsFromAgent(fromTime, toTime);
        //
        //        // Assert - сравнение ожидаемый результат с тестовым
        //        _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
        //    }
        //}
        //public class RamMetricsControllerUnitTests
        //{
        //    private RamMetricsController controller;
        //    public RamMetricsControllerUnitTests()
        //    {
        //        controller = new RamMetricsController();//экземпляр контролера,который тестируем
        //    }
        //
        //    [Fact]
        //    public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
        //    {
        //        //Arrange - задаем входные параметры теста
        //        //var agentId = 1;
        //        var fromTime = TimeSpan.FromSeconds(0);
        //        var toTime = TimeSpan.FromSeconds(100);
        //
        //        //Act - выполняем тест
        //        var result = controller.GetMetricsFromAgent(fromTime, toTime);
        //
        //        // Assert - сравнение ожидаемый результат с тестовым
        //        _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
        //    }
        //}
    }
}
