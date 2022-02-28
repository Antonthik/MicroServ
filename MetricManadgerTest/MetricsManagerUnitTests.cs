using MicroServ.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricManadgerTest
{
    public class MetricsManagerUnitTests
    {
        /// <summary>
        /// Test cpu
        /// </summary>
        public class CpuMetricsControllerUnitTests
        {
            private CpuMetricsController controller;


            public CpuMetricsControllerUnitTests()
            {
                controller = new CpuMetricsController();//экземпляр контролера,который тестируем
            }

            [Fact]
            public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
            {
                //Arrange - задаем входные параметры теста
                var agentId = 1;
                var fromTime = TimeSpan.FromSeconds(0);
                var toTime = TimeSpan.FromSeconds(100);

                //Act - выполняем тест
                var result = controller.GetMetricsFromAgent(agentId, fromTime, toTime);

                // Assert - сравнение ожидаемый результат с тестовым
                _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
            }
        }
        public class DotNetMetricsControllerUnitTests
        {
            private DotNetMetricsController controller;
            public DotNetMetricsControllerUnitTests()
            {
                controller = new DotNetMetricsController();//экземпляр контролера,который тестируем
            }

            [Fact]
            public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
            {
                //Arrange - задаем входные параметры теста
                //var agentId = 1;
                var fromTime = TimeSpan.FromSeconds(0);
                var toTime = TimeSpan.FromSeconds(100);

                //Act - выполняем тест
                var result = controller.GetMetricsFromAgent(fromTime, toTime);

                // Assert - сравнение ожидаемый результат с тестовым
                _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
            }
        }
        public class NetMetricsControllerUnitTests
        {
            private NetMetricsController controller;
            public NetMetricsControllerUnitTests()
            {
                controller = new NetMetricsController();//экземпляр контролера,который тестируем
            }

            [Fact]
            public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
            {
                //Arrange - задаем входные параметры теста
                //var agentId = 1;
                var fromTime = TimeSpan.FromSeconds(0);
                var toTime = TimeSpan.FromSeconds(100);

                //Act - выполняем тест
                var result = controller.GetMetricsFromAgent(fromTime, toTime);

                // Assert - сравнение ожидаемый результат с тестовым
                _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
            }
        }
        public class HddMetricsControllerUnitTests
        {
            private HddMetricsController controller;
            public HddMetricsControllerUnitTests()
            {
                controller = new HddMetricsController();//экземпляр контролера,который тестируем
            }

            [Fact]
            public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
            {
                //Arrange - задаем входные параметры теста
                //var agentId = 1;
                var fromTime = TimeSpan.FromSeconds(0);
                var toTime = TimeSpan.FromSeconds(100);

                //Act - выполняем тест
                var result = controller.GetMetricsFromAgent(fromTime, toTime);

                // Assert - сравнение ожидаемый результат с тестовым
                _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
            }
        }
        public class RamMetricsControllerUnitTests
        {
            private RamMetricsController controller;
            public RamMetricsControllerUnitTests()
            {
                controller = new RamMetricsController();//экземпляр контролера,который тестируем
            }

            [Fact]
            public void GetMetricsFromAgent_ReturnsOk() // дополняем через "_", что ожидаем - OKг
            {
                //Arrange - задаем входные параметры теста
                //var agentId = 1;
                var fromTime = TimeSpan.FromSeconds(0);
                var toTime = TimeSpan.FromSeconds(100);

                //Act - выполняем тест
                var result = controller.GetMetricsFromAgent(fromTime, toTime);

                // Assert - сравнение ожидаемый результат с тестовым
                _ = Assert.IsAssignableFrom<IActionResult>(result); // _= - нижний прочерк означает, что результат выражения(значение переменной) не используем.
            }
        }
    }
}
