using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkTracker.Business;
using NSubstitute;
using WorkTracker.Repositories;
using WorkTracker.Entities;
using System.Collections.Generic;

namespace WorkTracker.Tests
{
    [TestClass]
    public class StatsCalculatorTests
    {
        [TestMethod]
        public void Should_Correctly_Calculate_Stats()
        {
            var todayDate = new DateTime(2014, 10, 24);
            var stateChangeRepository = Substitute.For<IStateChangeRepository>();

            var stateChanges = new List<StateChange>();
            stateChanges.Add(new StateChange(StateNamesEnum.Break, todayDate.AddDays(-1).AddHours(23).AddMinutes(40), null));
            stateChanges.Add(new StateChange(StateNamesEnum.Stopped, todayDate.AddHours(1).AddMinutes(10), stateChanges.Last()));
            stateChanges.Add(new StateChange(StateNamesEnum.Work, todayDate.AddHours(6), stateChanges.Last()));
            stateChanges.Add(new StateChange(StateNamesEnum.Stopped, todayDate.AddHours(12), stateChanges.Last()));
            stateChanges.Add(new StateChange(StateNamesEnum.Work, todayDate.AddHours(23).AddMinutes(30), stateChanges.Last()));

            stateChangeRepository.GetByDate(todayDate).Returns(stateChanges);

            var stats = new StatsCalculator().GetSingleDayStats(stateChangeRepository.GetByDate(todayDate));

            Assert.AreEqual(new TimeSpan(6, 30, 0), stats.WorkTime);
            Assert.AreEqual(new TimeSpan(1, 10, 0), stats.BreakTime);
            Assert.AreEqual(todayDate.AddHours(6), stats.WorkStart);
            Assert.AreEqual(todayDate.AddDays(1).AddMinutes(-1), stats.WorkEnd);
        }
    }
}
