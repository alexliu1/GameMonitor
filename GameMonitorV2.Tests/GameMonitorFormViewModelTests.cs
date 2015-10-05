using System;
using GameMonitorV2.Tests.Fakes;
using GameMonitorV2.ViewModel;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorFormViewModelTests
    {
        [Test]
        public void WhenChoosingAProgramToMonitor()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(true);

            var unit = new GameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s, a) => { propertyName = a.PropertyName; };

            unit.GameName = "notepad";
            Assert.That(propertyName, Is.EqualTo("GameName"));

            unit.ElapsedTime = TimeSpan.FromSeconds(1);
            Assert.That(propertyName, Is.EqualTo("ElapsedTime"));
        }
    }
}