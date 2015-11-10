using System;
using System.ComponentModel;
using GameMonitorV2.Model;
using GameMonitorV2.ViewModel;
using log4net;
using Moq;

namespace GameMonitorV2.Tests
{
    class TestClassFactory
    {
        public static GameMonitorFormViewModel CreateGamMonitorFormViewModel(Func<Type, ILog> loggerFactory = null)
        {
            if (loggerFactory == null)
                loggerFactory = (type) => new Mock<ILog>().Object;

            return new GameMonitorFormViewModel(loggerFactory);
        }

        public static GameMonitorDisplayViewModel CreateGameMonitorDisplayViewModel(ISynchronizeInvoke synchronizeInvoke = null, string processName = null, Func<Type, ILog> loggerFactory = null)
        {
            if (synchronizeInvoke == null)
                synchronizeInvoke = new Mock<ISynchronizeInvoke>().Object;

            if (processName == null)
                processName = "notepad.exe";

            if (loggerFactory == null)
                loggerFactory = (type) => new Mock<ILog>().Object;

            return new GameMonitorDisplayViewModel(synchronizeInvoke, processName, loggerFactory);
        }

        public static PollWatcher CreatePollWatcher(string gameName, Func<Type, ILog> loggerFactory = null)
        {
            if (gameName == null)
                gameName = "notepad.exe";

            if (loggerFactory == null)
                loggerFactory = (type) => new Mock<ILog>().Object;

            return new PollWatcher(gameName, loggerFactory);
        }
    }
}