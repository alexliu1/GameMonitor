using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameMonitorV2.View;
using GameMonitorV2.ViewModel;
using NUnit.Framework;
using Timer = System.Timers.Timer;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorDisplayViewModelTests
    {
        [Test]
        public void WhenGameNameIsSet_ThenPropertyChangedIsRaisedCorrectly()
        {
            var propertyChangedIsCalled = false;
            var form = new GameMonitorDisplay("notepad.exe");
            
            var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
            unit.PropertyChanged += delegate { propertyChangedIsCalled = true; };
            unit.GameName = "notepad";

            Assert.That(propertyChangedIsCalled, Is.True);
        }

        [Test]
        public void WhenRunningTimeIsSetViaATimer_ThenPropertyChangedIsRaisedCorrectedly()
        {
            var propertyChangedIsCalled = false;
            var form = new GameMonitorDisplay("notepad.exe");

            var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
            unit.PropertyChanged += delegate { propertyChangedIsCalled = true; };
            unit.ElapsedTime = TimeSpan.FromMilliseconds(1000);

            Assert.That(propertyChangedIsCalled, Is.True);
        }

        [Test]
        public void WhenTimeLimitIsUpTimeExpiredEventIsTriggered()
        {
            var timeExpiredEventIsTriggered = false;
            var form = new GameMonitorDisplay("notepad.exe");

            var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
            unit.TimeExpired += delegate { timeExpiredEventIsTriggered = true; };
            unit.ElapsedTime = TimeSpan.FromHours(3.017);

            Assert.That(timeExpiredEventIsTriggered, Is.True);
        }

        [Test]
        public void CloseProgramWillCloseTheRightProgram()
        {
            Process process = null;

            try
            {
                var closeTheRightProgram = false;
                var form = new GameMonitorDisplay("notepad.exe");
                process = Process.Start("notepad.exe");
                
                var unit = new GameMonitorDisplayViewModel(form, "notepad.exe");
                unit.CloseProgram();
                Thread.Sleep(1000); //Give program time to exit
                
                if (process.HasExited == true) closeTheRightProgram = true;
                Assert.That(closeTheRightProgram, Is.True);
            }
            finally
            {
                if(!process.HasExited) process.Kill();
            }
            

        }
    }
}
