using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class GameMonitorFormViewModelTests
    {
        [Test]
        public void WhenGameNameIsSet_ThenPropertyChangedIsRaisedCorrectly()
        {
            var propertyChangedIsCalled = false;
            var form = new GameMonitorForm();
            
            var unit = new GameMonitorFormViewModel(form);
            unit.PropertyChanged += delegate { propertyChangedIsCalled = true; };
            unit.GameName = "notepad";

            Assert.That(propertyChangedIsCalled, Is.True);
        }

        [Test]
        public void WhenRunningTimeIsSetViaATimer_ThenPropertyChangedIsRaisedCorrectedly()
        {
            var propertyChangedIsCalled = false;
            var form = new GameMonitorForm();

            var unit = new GameMonitorFormViewModel(form);
            unit.PropertyChanged += delegate { propertyChangedIsCalled = true; };
            var timer = new Timer() {Interval = 1000};
            timer.Elapsed += delegate { unit.ElapsedTime = unit.ElapsedTime.Add(TimeSpan.FromMilliseconds(timer.Interval)); };
            timer.Start();
            Thread.Sleep(1500);

            Assert.That(propertyChangedIsCalled, Is.True);
            timer.Stop();
        }

        [Test]
        public void RunningTimerIsGivingTheRightTime()
        {
            
        }
    }
}
