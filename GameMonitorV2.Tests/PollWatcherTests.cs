using System;
using System.Diagnostics;
using System.Threading;
using GameMonitorV2.Model;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class PollWatcherTests
    {
        [Test]
        public void WhenTwoSequentialProcesses_ThenPollWatcherCanAccuratelyMeasureElapsedTimeOfBoth()
        {
            var pollwatcher = CreatePollWatcher("notepad");
            Process process = null;
            Process process2 = null;

            try
            {
                process = Process.Start("notepad.exe");
                Thread.Sleep(3000);
                if (process != null) process.Kill();

                Thread.Sleep(1000); //Give Windows 1 second to close notepad

                process2 = Process.Start("notepad.exe");
                Thread.Sleep(2100); //Give PollWatcher extra 100ms to record next tick
                if (process2 != null) process2.Kill();

                var actual = pollwatcher.ElapsedTime;

                var expected = new TimeSpan(0, 0, 5); //0 minutes 5 seconds
                Assert.AreEqual(expected, actual);
                Console.WriteLine("{0}\n{1}", expected, actual);
            }
            finally
            {
                KillProcess(process);
                KillProcess(process2);
            }
        }

        [Test]
        public void GivenASingleProcess_ThenPollWatcherCanMeasureAccuratelyElapsedTime()
        {
            var pollwatcher = CreatePollWatcher("notepad");
            Process process = null;

            try
            {
                process = Process.Start("notepad.exe");
                Thread.Sleep(3100); //Give PollWatcher 100ms to record next tick
                if (process != null) process.Kill();

                var actual = pollwatcher.ElapsedTime;

                var expected = new TimeSpan(0, 0, 3); // 0 minutes 3 seconds
                Assert.AreEqual(expected, actual);
                Console.WriteLine("{0}\n{1}", expected, actual);
            }
            finally
            {
                KillProcess(process);
            }
        }

        [Test]
        public void WhenThereAreTwoConcurrentProcesses_ThenPollWatcherCanMeasureElapsedTime()
        {
            var pollwatcher1 = CreatePollWatcher("notepad");
            var pollwatcher2 = CreatePollWatcher("wordpad");
            Process process1 = null;
            Process process2 = null;

            try
            {
                process1 = Process.Start("notepad.exe");
                process2 = Process.Start("wordpad.exe");
                Thread.Sleep(3100); //Give PollWatcher 100ms to record next tick
                if (process1 != null) process1.Kill();
                Thread.Sleep(2100); //Give PollWatcher 100ms to record next tick
                if (process2 != null) process2.Kill();

                var actual1 = pollwatcher1.ElapsedTime;
                var actual2 = pollwatcher2.ElapsedTime;

                var expected1 = new TimeSpan(0, 0, 3); // 0 minutes 3 seconds
                var expected2 = new TimeSpan(0, 0, 5); // 0 minutes 5 seconds
                Assert.AreEqual(expected1, actual1);
                Assert.AreEqual(expected2, actual2);

                Console.WriteLine("{0}\n{1}", expected1, actual1);
                Console.WriteLine("{0}\n{1}", expected2, actual2);

            }
            finally
            {
                KillProcess(process1);
                KillProcess(process2);

            }
        }

        private static void KillProcess(Process process)
        {
            if (process != null && !process.HasExited)
                process.Kill();
        }

        public static PollWatcher CreatePollWatcher(string gameName, Func<Type, ILog> loggerFactory = null)
        {
            if (gameName == null)
                gameName = "notepad.exe";

            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            return new PollWatcher(gameName, loggerFactory);
        }
    }
}