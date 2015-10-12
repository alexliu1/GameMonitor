using System;
using System.ComponentModel;
using GameMonitorV2.Tests.Fakes;
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
        public void WhenChoosingAProgramToMonitor()
        {
            var unit = CreateUnit();
            unit.ShouldMonitor("notepad.exe");

            Assert.IsTrue(unit.ShouldMonitor("wordpad.exe"));

            Assert.IsFalse(unit.ShouldMonitor("notepad.exe"));
        }

        private GameMonitorFormViewModel CreateUnit(ILog logger = null)
        {
            if (logger == null)
                logger = new Mock<ILog>().Object;

            return new GameMonitorFormViewModel(logger);
        }
    }
}