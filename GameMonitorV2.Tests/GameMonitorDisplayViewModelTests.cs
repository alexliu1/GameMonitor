using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using GameMonitorV2.Model;
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
        public void WhenGameNamePropertyIsSet_ThenPropertyChangedIsRaisedCorrectly()
        {
            var propertyName = string.Empty;

            var unit = CreateGameMonitorDisplayViewModel();
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };
            
            unit.GameName = "notepad";
            Assert.That(propertyName, Is.EqualTo("GameName"));

            unit.ElapsedTime = TimeSpan.FromSeconds(1);
            Assert.That(propertyName, Is.EqualTo("ElapsedTime"));
        }

        [Test]
        public void WhenAnyPropertiesAreChanged_ThenPropertyChangedIsRaisedCorrectly()
        {
            var propertyName = string.Empty;
            var synchronizeInvoke = new FakeSynchronizeInvoke(false);
            
            var unit = CreateGameMonitorDisplayViewModel(synchronizeInvoke, "notepad.exe");
            unit.PropertyChanged += (s,a) => { propertyName = a.PropertyName; };

            var properties = typeof(GameMonitorDisplayViewModel).GetProperties();
            foreach (var property in properties)
            {
                propertyName = string.Empty;

                var propertyNameToSet = property.Name;
                var propertyTypeToSet = property.PropertyType;
                var propertyValueToSet = GetValue(propertyTypeToSet);

                Console.WriteLine("Setting property [{0}] of type [{1}] to value [{2}]", propertyNameToSet, propertyTypeToSet, propertyValueToSet);

                property.SetValue(unit, propertyValueToSet);
                Assert.That(propertyName, Is.EqualTo(propertyNameToSet));
            }
        }

        private static object GetValue(Type propertyType)
        {
            if (propertyType == typeof(int))
                return 1;

            if (propertyType == typeof(string))
                return "a";

            if (propertyType == typeof(TimeSpan))
                return TimeSpan.FromSeconds(1);

            return null;
        }

        [Test]
        public void WhenTimeLimitIsUp_ThenTimeExpiredEventIsRaised()
        {
            var timeExpiredEventIsTriggered = false;
            var iSynchronizeInvoke = new Mock<ISynchronizeInvoke>() { CallBase = true };
            iSynchronizeInvoke.Setup(m => m.InvokeRequired).Returns(false);

            var unit = CreateGameMonitorDisplayViewModel(iSynchronizeInvoke.Object, "notepad.exe");
            unit.TimeExpired += delegate { timeExpiredEventIsTriggered = true; };
            unit.ElapsedTime = TimeSpan.FromHours(3.017);  // time limit is hard-coded to 3 hours in GameMonitorDisplayViewModel

            Assert.That(timeExpiredEventIsTriggered, Is.True);
        }

        [Test]
        public void WhenCloseProgram_ThenTheCorrectProgramIsClosed()
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

                var unit = CreateGameMonitorDisplayViewModel(iSynchronizeInvoke.Object, processName);
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

        private GameMonitorDisplayViewModel CreateGameMonitorDisplayViewModel(ISynchronizeInvoke synchronizeInvoke = null, string processName = null, PollWatcherFactory pollWatcherFactory = null, Func<Type, ILog> loggerFactory = null)
        {
            if (synchronizeInvoke == null)
                synchronizeInvoke = new Mock<ISynchronizeInvoke>().Object;

            if (processName == null)
                processName = "notepad.exe";

            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            if (pollWatcherFactory == null)
                pollWatcherFactory = new PollWatcherFactory(loggerFactory);

            return new GameMonitorDisplayViewModel(synchronizeInvoke, processName, pollWatcherFactory, loggerFactory);
        }
    }
}
