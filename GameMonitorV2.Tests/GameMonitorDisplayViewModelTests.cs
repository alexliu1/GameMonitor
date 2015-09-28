using System;
using System.Diagnostics;
using System.Threading;
using GameMonitorV2.View;
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
            var form = new GameMonitorDisplay("notepad.exe");
            
            var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };
            unit.GameName = "notepad";

            Assert.That(propertyName, Is.EqualTo("GameName"));
        }

        [Test]
        public void WhenRunningTimeIsSet_ThenPropertyChangedIsRaisedCorrectedly()
        {
            var propertyChangedIsCalled = false;
            var form = new GameMonitorDisplay("notepad.exe");

            var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
            unit.PropertyChanged += delegate { propertyChangedIsCalled = true; };
            unit.ElapsedTime = TimeSpan.FromMilliseconds(1000);

            Assert.That(propertyChangedIsCalled, Is.True);
        }

        [Test]
        public void WhenTimeLimitIsUp_TimeExpiredEventIsRaised()
        {
            var timeExpiredEventIsTriggered = false;
            var form = new GameMonitorDisplay("notepad.exe");

            var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
            unit.TimeExpired += delegate { timeExpiredEventIsTriggered = true; };
            unit.ElapsedTime = TimeSpan.FromHours(3.017);  // time limit is hard-coded to X in Y

            Assert.That(timeExpiredEventIsTriggered, Is.True);
        }

        [Test]
        public void WhenCloseProgram_TheCorrectProgramIsClosed()
        {
            const string processName = "notepad.exe";
            Process process = null;

            try
            {
                var form = new GameMonitorDisplay(processName);
                process = Process.Start(processName);
                
                if (process == null)
                    Assert.Fail("Unable to start process [{0}]", processName);

                var unit = new GameMonitorDisplayViewModel(form, processName);
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
