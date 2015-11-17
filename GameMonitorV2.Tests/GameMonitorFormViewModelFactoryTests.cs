using System;
using GameMonitorV2.ViewModel;
using log4net;
using Moq;
using NUnit.Framework;

namespace GameMonitorV2.Tests
{
    [TestFixture]
    public class GameMonitorFormViewModelFactoryTests
    {
        [Test]
        public void WhenTheFormViewModelFactoryCreateMethodIsCalled_ThenANewGameMonitorFormViewModelIsCreated()
        {
            var unit = CreateUnit();
            var formViewModel = unit.CreateNewFormViewModel();

            Assert.IsTrue(formViewModel.GetType() == typeof(GameMonitorFormViewModel));
        }

        private GMFViewModelFactory CreateUnit(Func<Type, ILog> loggerFactory = null)
        {
            if (loggerFactory == null)
                loggerFactory = type => new Mock<ILog>().Object;

            return new GMFViewModelFactory(loggerFactory);
        }
    }
}