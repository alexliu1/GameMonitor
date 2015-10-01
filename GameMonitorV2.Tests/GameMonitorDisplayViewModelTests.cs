using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using GameMonitorV2.Tests.Fakes;
using GameMonitorV2.ViewModel;
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
            
            var unit = new GameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };
            
            unit.GameName = "notepad";
            Assert.That(propertyName, Is.EqualTo("GameName"));

            unit.ElapsedTime = TimeSpan.FromSeconds(1);
            Assert.That(propertyName, Is.EqualTo("ElapsedTime"));
        }

        [Test]
        public void WhenGameNameIsSet_ThenPropertyChangedIsRaisedCorrectly_WhenInvokeIsNotRequired()
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

        [Test]
        public void WhenGameNameIsSet_ThenPropertyChangedIsRaisedCorrectlyWhenInvokeIsNotRequired()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(false);

            var unit = new GameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s, a) => { propertyName = a.PropertyName; };
            unit.GameName = "notepad";

            Assert.That(propertyName, Is.EqualTo("GameName"));
        }

        [Test]
        public void WhenRunningTimeIsSet_ThenPropertyChangedIsRaisedCorrectedlyWhenInvokeIsRequired()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(false);

            var unit = new GameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s, a) => { propertyName = a.PropertyName; };
            unit.ElapsedTime = TimeSpan.FromMilliseconds(1000);

            Assert.That(propertyName, Is.EqualTo("ElapsedTime"));
        }

        [Test]
        public void WhenRunningTimeIsSet_ThenPropertyChangedIsRaisedCorrectedlyWhenInvokeIsNotRequired()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(false);

            var unit = new GameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s, a) => { propertyName = a.PropertyName; };
            unit.ElapsedTime = TimeSpan.FromMilliseconds(1000);

            Assert.That(propertyName, Is.EqualTo("ElapsedTime"));
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
    }
}
