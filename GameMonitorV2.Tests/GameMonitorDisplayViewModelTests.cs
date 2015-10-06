using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using GameMonitorV2.Tests.Fakes;
using GameMonitorV2.ViewModel;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorDisplayViewModelTests
    {
        [Test]
        public void WhenGameNameIsSet_ThenPropertyChangedIsRaisedCorrectly_WhenInvokeIsRequired()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(true);

            var unit = CreateUnit();
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };
            
            unit.GameName = "notepad";
            Assert.That(propertyName, Is.EqualTo("GameName"));

            unit.ElapsedTime = TimeSpan.FromSeconds(1);
            Assert.That(propertyName, Is.EqualTo("ElapsedTime"));
        }

        [Test]
        public void WhenPropertiesAreChanged_PropertyChangedIsRaisedCorrectly_AndWhenInvokeIsNotRequired()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(false);
            
            var unit = new GameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };

            foreach (var property in typeof(GameMonitorDisplayViewModel).GetProperties())
            {
                property.SetValue(unit, GetValue(property.GetType()));
                Assert.That(propertyName, Is.EqualTo(property.Name));
            }
        }

        private static object GetValue(Type propertyType)
        {
            if (propertyType == typeof(int))
                return 1;
            if (propertyType == typeof(string))
                return "GameName";
            if (propertyType == typeof(TimeSpan))
                return TimeSpan.FromSeconds(1);
            return null;
        }

        [Test]
        public void WhenTimeLimitIsUp_TimeExpiredEventIsRaised()
        {
            var timeExpiredEventIsTriggered = false;
            var iSynchronizeInvoke = new Mock<ISynchronizeInvoke>() { CallBase = true };
            iSynchronizeInvoke.Setup(m => m.InvokeRequired).Returns(false);

            var unit = new GameMonitorDisplayViewModel(iSynchronizeInvoke.Object, "notepad.exe");
            unit.TimeExpired += delegate { timeExpiredEventIsTriggered = true; };
            unit.ElapsedTime = TimeSpan.FromHours(3.017);  // time limit is hard-coded to 3 hours in GameMonitorDisplayViewModel

            Assert.That(timeExpiredEventIsTriggered, Is.True);
        }

        [Test]
        public void WhenCloseProgram_TheCorrectProgramIsClosed()
        {
            const string processName = "notepad.exe";
            Process process = null;

            try
            {
                var iSynchronizeInvoke = new Mock<ISynchronizeInvoke>() { CallBase = true };
                iSynchronizeInvoke.Setup(m => m.InvokeRequired).Returns(false);
                process = Process.Start(processName);
                
                if (process == null)
                    Assert.Fail("Unable to start process [{0}]", processName);

                var unit = new GameMonitorDisplayViewModel(iSynchronizeInvoke.Object, processName);
                unit.CloseProgram();
                Thread.Sleep(1000); //Give program time to exit
                
                Assert.That(process.HasExited, Is.True);
            }
            finally
            {
                if(process != null && !process.HasExited) 
                    process.Kill();
            }
        }

        private GameMonitorDisplayViewModel CreateUnit(ISynchronizeInvoke synchronizeInvoke = null, string processName = null, ILog logger = null)
        {
            if (synchronizeInvoke == null)
                synchronizeInvoke = new Mock<ISynchronizeInvoke>().Object;

            if (processName == null)
                processName = "notepad.exe";

            if (logger == null)
                logger = new Mock<ILog>().Object;

            return new GameMonitorDisplayViewModel(synchronizeInvoke, processName, logger);
        }

    }
}
