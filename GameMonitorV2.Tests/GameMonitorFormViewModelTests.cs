using System;
using GameMonitorV2.ViewModel;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorFormViewModelTests
    {
        [Test]
        public void WhenChoosingAProgramToMonitor_ThenDuplicatesAreRejected()
        {
            var unit = CreateGamMonitorFormViewModel();
            unit.ShouldMonitor("notepad.exe");

            Assert.IsTrue(unit.ShouldMonitor("wordpad.exe"));

            Assert.IsFalse(unit.ShouldMonitor("notepad.exe"));
        }

        private GameMonitorFormViewModel CreateGamMonitorFormViewModel(Func<Type, ILog> loggerFactory = null)
        {
            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            return new GameMonitorFormViewModel(loggerFactory);
        }
    }
}