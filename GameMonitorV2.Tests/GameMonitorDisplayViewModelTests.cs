using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using GameMonitorV2.View;
using GameMonitorV2.ViewModel;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorDisplayViewModelTests
    {
        [Test]
        public void WhenGameNameIsSet_ThenPropertyChangedIsRaisedCorrectly()
        {
            var propertyName = string.Empty;
            var iSynchronizeInvoke = new Mock<ISynchronizeInvoke>() {CallBase = true};
            iSynchronizeInvoke.Setup(m => m.InvokeRequired).Returns(false);
            //iSynchronizeInvoke.Setup(m => m.BeginInvoke(It.IsAny<Delegate>(), It.IsAny<Object[]>())).Callback(() => It.IsAny<Delegate>());
            
            var unit = new GameMonitorDisplayViewModel(iSynchronizeInvoke.Object, "notepad.exe");
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };
            unit.GameName = "notepad";

            Assert.That(propertyName, Is.EqualTo("GameName")); 
        }

        [Test]
        public void WhenRunningTimeIsSet_ThenPropertyChangedIsRaisedCorrectedly()
        {
            var propertyName = string.Empty;
            var iSynchronizeInvoke = new Mock<ISynchronizeInvoke>() { CallBase = true };
            iSynchronizeInvoke.Setup(m => m.InvokeRequired).Returns(false);

            var unit = new GameMonitorDisplayViewModel(iSynchronizeInvoke.Object, "notepad.exe");
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
