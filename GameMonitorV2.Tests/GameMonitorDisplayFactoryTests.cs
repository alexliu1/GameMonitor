using System;
using GameMonitorV2.Model;
using GameMonitorV2.View;
using GameMonitorV2.ViewModel;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorDisplayFactoryTests
    {
        [Test]
        public void WhenUsingGameMonitorDisplayFactory_ThenANewGameMonitorDisplayIsCreated()
        {
            var unit = CreateUnit();
            var newDisplay = unit.CreateNewDisplay("notepad.exe");

            Assert.IsTrue(newDisplay.GetType() == typeof(GameMonitorDisplay));
        }

        private GameMonitorDisplayFactory CreateUnit(GMDViewModelFactory gameMonitorDisplayViewModelFactory = null, Func<Type, ILog> loggerFactory = null)
        {
            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            if (gameMonitorDisplayViewModelFactory == null)
            {
                var pollWatcherFactory = new PollWatcherFactory(loggerFactory);
                gameMonitorDisplayViewModelFactory = new GMDViewModelFactory(pollWatcherFactory, loggerFactory);
            }

            return new GameMonitorDisplayFactory(gameMonitorDisplayViewModelFactory, loggerFactory);
        }
    }
}