using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WeatherData.Business;

namespace LightManager.UnitTests
{
    [TestClass]
    public class LightTests
    {
        private Mock<ITelldus> TelldusMock { get; set; }
        private Mock<IWeatherProvider> WeatherProviderMock { get; set; }
        private WeatherData.Business.LightManager LightManager { get; set; }

        [TestInitialize]
        public void Setup()
        {
            TelldusMock = new Mock<ITelldus>();
            WeatherProviderMock = new Mock<IWeatherProvider>();
            LightManager = new WeatherData.Business.LightManager(TelldusMock.Object, WeatherProviderMock.Object);
            WeatherProviderMock.Setup(m => m.GetSunsetTime()).Returns(new DateTime(2017, 9, 4, 19, 30, 0));
        }

        [TestMethod]
        public void GivenItsTimeToTurnOnLamps_WhenTurningOnLamps_ThenOnlyMainSectionIsTurnedOn()
        {
            // Arrange
            LightManager.DateTimeNow = new DateTime(2017, 9, 4, 19, 10, 0);

            // Act 
            LightManager.TimerElapsed();

            //Result
            TelldusMock.Verify(m => m.TurnOn("lampor"), Times.Once());
        }

        [TestMethod]
        public void GivenItsWeekday_WhenTicks_ThenShutOffLamps()
        {
            // Arrange
            LightManager.DateTimeNow = new DateTime(2017, 9, 4, 19, 10, 0);
            LightManager.TimerElapsed();
            TelldusMock.Reset();

            LightManager.DateTimeNow = new DateTime(2017, 9, 4, 21, 41, 0);

            // Act 
            LightManager.TimerElapsed();

            //Result
            TelldusMock.Verify(m => m.TurnOff("lampor"), Times.Once());
            TelldusMock.Verify(m => m.TurnOn("Hall"), Times.Once());
        }

        [TestMethod]
        public void GivenNightHasPassed_WhenNewDay_ThenLampsNotTurnedOn()
        {
            // Arrange
            LightManager.DateTimeNow = new DateTime(2017, 9, 4, 19, 10, 0);
            LightManager.TimerElapsed();


            LightManager.DateTimeNow = new DateTime(2017, 9, 4, 22, 10, 0);
            LightManager.TimerElapsed();

            LightManager.DateTimeNow = new DateTime(2017, 9, 5, 0, 0, 0);

            //Act
            TelldusMock.Reset();
            LightManager.TimerElapsed();

            //Verify
            TelldusMock.Verify(m => m.TurnOn(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GivenItIsWeekend_WhenTimeElapsed_ThenHandleHallOnce()
        {
            // Arrange
            WeatherProviderMock.Setup(m => m.GetSunsetTime()).Returns(new DateTime(2017, 9, 15, 19, 30, 0));
            LightManager.DateTimeNow = new DateTime(2017, 9, 15, 19, 10, 0);
            LightManager.TimerElapsed();
            TelldusMock.Verify(m => m.TurnOn("lampor"), Times.Once());
            TelldusMock.Reset();


            LightManager.DateTimeNow = new DateTime(2017, 9, 15, 21, 41, 0);
            LightManager.TimerElapsed();
            TelldusMock.Verify(m => m.TurnOff("Sovrum"), Times.Once);
            TelldusMock.Verify(m => m.TurnOff("Hall"), Times.Never);
            TelldusMock.Verify(m => m.TurnOn("Hall"), Times.Never);
            TelldusMock.Verify(m => m.TurnOff("lampor"), Times.Never);
            TelldusMock.Verify(m => m.TurnOn("lampor"), Times.Never);

            TelldusMock.Reset();

            LightManager.DateTimeNow = new DateTime(2017, 9, 16, 03, 01, 0);
            LightManager.TimerElapsed();
            TelldusMock.Verify(m => m.TurnOff("lampor"), Times.Once);
        }
    }
}
