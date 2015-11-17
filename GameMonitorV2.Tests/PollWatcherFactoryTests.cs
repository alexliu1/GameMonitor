using System;
using GameMonitorV2.Model;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class PollWatcherFactoryTests
    {
        [Test]
        public void WhenThePollWatcherFactoryCreateMethodIsCalled_ThenANewGameMonitorFormViewModelIsCreated()
        {
            var unit = CreateUnit();
            var pollWatcher = unit.CreateNewPollWatcher("notepad");

            Assert.IsTrue(pollWatcher.GetType() == typeof(PollWatcher));
        }

        private PollWatcherFactory CreateUnit(Func<Type, ILog> loggerFactory = null)
        {
            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            return new PollWatcherFactory(loggerFactory);
        }
    }
}