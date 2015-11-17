using System;
using System.ComponentModel;
using GameMonitorV2.Model;
using GameMonitorV2.ViewModel;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorDisplayViewModelFactoryTests
    {
        [Test]
        public void WhenUsingGMDViewModelFactory_ThenAGameMonitorDisplayViewModelIsGenerated()
        {
            var synchronizeInvoke = new Mock<ISynchronizeInvoke>().Object;
            var processName = "notepad.exe";


            var unit = CreateUnit();
            unit.CreateNewDisplayViewModel(synchronizeInvoke, processName);
            var displayViewModel = unit.CreateNewDisplayViewModel(synchronizeInvoke, processName);

            Assert.IsTrue(displayViewModel.GetType() == typeof(GameMonitorDisplayViewModel));
        }

        private GMDViewModelFactory CreateUnit(PollWatcherFactory pollWatcherFactory = null, Func<Type, ILog> loggerFactory = null)
        {
            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            if (pollWatcherFactory == null)
                pollWatcherFactory = new PollWatcherFactory(loggerFactory);

            return new GMDViewModelFactory(pollWatcherFactory, loggerFactory);
        }
    }
}