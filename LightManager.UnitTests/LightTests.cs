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

        [TestMethod]
        public void GivenKlockHasPassedMidNight_WhenGivingPule_ThenShutOfAllLamps()
        {
            // Arrange
            

            // Act 

            //Result
        }
    }
}
