using System;
using System.Diagnostics;
using System.Threading;
using Game_Time_Monitor;
using NUnit.Framework;

namespace Project1
{
    [TestFixture]
    public class PollWatcher3Tests
    {
        private double tolerance = 1 * 1000;

        [Test]
        public void CanAccuratelyMeasureElapsedTimeOfTwoSequentialProcesses()
        {
            var pollwatcher = new PollWatcher3("notepad");
            Process process = null;
            Process process2 = null;

            int foo;

            try
            {
                process = Process.Start("notepad.exe");
                Thread.Sleep(3000);
                process.Kill();

                Thread.Sleep(1000); //Give Windows 1 second to close notepad

                process2 = Process.Start("notepad.exe");
                Thread.Sleep(2000);
                process2.Kill();

                var actual = pollwatcher.ElapsedTime;

                const double expected = 5 * 1000;
                Assert.AreEqual(expected, actual, tolerance);
                Console.WriteLine(expected + "\n " + actual);
            }
            finally
            {
                KillProcess(process);
                KillProcess(process2);
            }
        }

        [Test]
        public void CanMeasureAccuratelyElapsedTimeOfASingleProcess()
        {
            var pollwatcher = new PollWatcher3("notepad");
            Process process = null;

            try
            {
                process = Process.Start("notepad.exe");
                Thread.Sleep(3000);
                process.Kill();

                var actual = pollwatcher.ElapsedTime;

                const double expected = 3 * 1000;
                Assert.AreEqual(expected, actual, tolerance);
                Console.WriteLine(expected + "\n " + actual);
            }
            finally
            {
                KillProcess(process);
            }
        }

        [Test]
        public void CanMeasureElapsedTimeOfTwoConcurrentProcesses()
        {
            var pollwatcher1 = new PollWatcher3("notepad");
            var pollwatcher2 = new PollWatcher3("wordpad");
            Process process1 = null;
            Process process2 = null;

            try
            {
                process1 = Process.Start("notepad.exe");
                process2 = Process.Start("wordpad.exe");
                Thread.Sleep(3000);
                process1.Kill();
                Thread.Sleep(2000);
                process2.Kill();

                var actual1 = pollwatcher1.ElapsedTime;
                var actual2 = pollwatcher2.ElapsedTime;

                const double expected1 = 3 * 1000;
                const double expected2 = 5 * 1000;
                Assert.AreEqual(expected1, actual1, tolerance);
                Assert.AreEqual(expected2, actual2, tolerance);

                Console.WriteLine(expected1 + "\n " + actual1);
                Console.WriteLine(expected2 + "\n " + actual2);

            }
            finally
            {
                KillProcess(process1);
                KillProcess(process2);

            }
        }

        [Test]
        public void GetFormattedElapsedTimeReturnsProperlyFormattedTime()
        {
            var pollwatcher = new PollWatcher3("notepad");
            Process process = null;

            try
            {
                process = Process.Start("notepad.exe");
                Thread.Sleep(3200);
                process.Kill();

                var actual = pollwatcher.GetFormattedElapsedTime;

                const string expected = "00:00:03";
                StringAssert.AreEqualIgnoringCase(expected, actual);
                Console.WriteLine(expected + "\n " + actual);
            }
            finally
            {
                KillProcess(process);
            }
        }

        private void KillProcess(Process process)
        {
            if (process != null && !process.HasExited)
                process.Kill();
        }


        
    }
}