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
        private WeatherData.Business.LightManager LightManager { get; set; }

        [TestInitialize]
        public void Setup()
        {
            TelldusMock = new Mock<ITelldus>();
            LightManager = new WeatherData.Business.LightManager(TelldusMock.Object);
        }

        [TestMethod]
        public void GivenKlockHasPassedMidNight_WhenGivingPule_ThenShutOfAllLamps()
        {
            // Arrange
            LightManager.DateTimeNow = new DateTime(2017, 9, 4, 3, 10, 0);

            // Act 
            LightManager.TimerElapsed();

            //Result
            TelldusMock.Verify(m => m.TurnOff(It.IsAny<string>()), Times.Once());
        }
    }
}
