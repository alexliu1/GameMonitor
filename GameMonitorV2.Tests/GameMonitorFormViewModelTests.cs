using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorFormViewModelTests
    {
        [Test]
        public void WhenChoosingAProgramToMonitor()
        {
            var unit = TestClassFactory.CreateGamMonitorFormViewModel();
            unit.ShouldMonitor("notepad.exe");

            Assert.IsTrue(unit.ShouldMonitor("wordpad.exe"));

            Assert.IsFalse(unit.ShouldMonitor("notepad.exe"));
        }
    }
}