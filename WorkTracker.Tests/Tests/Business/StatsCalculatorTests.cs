using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System;
using WorkTracker.Business;
using WorkTracker.Repositories;
using WorkTracker.Tests.Infrastructure;

namespace WorkTracker.Tests
{
    [TestClass]
    public class StatsCalculatorTests
    {
        [TestMethod]
        public void Should_Correctly_Calculate_Stats_For_Specific_Day()
        {
            // arrange
            var stateChangeRepository = Substitute.For<IStateChangeRepository>();
            stateChangeRepository.GetByDate("2014.10.24 12:00".Date())
                .Returns(DataBuilder.StateChangesCollection(
                    StateNamesEnum.Break.On(  "2014.10.23 23:40"),
                    StateNamesEnum.Stopped.On("2014.10.24  1:10"),
                    StateNamesEnum.Work.On(   "2014.10.24  6:00"),
                    StateNamesEnum.Stopped.On("2014.10.24 12:00"),
                    StateNamesEnum.Work.On(   "2014.10.24 23:30")
                ));

            // act
            var stats = new DailyStatsCalculator().GetSingleDayStats(stateChangeRepository.GetByDate("2014.10.24 12:00".Date()));

            // assert
            stats.WorkTime.ShouldBe(new TimeSpan(6, 30, 0));
            stats.BreakTime.ShouldBe(new TimeSpan(1, 10, 0));
            stats.WorkStart.ShouldBe("2014.10.24 6:00".Date());
            stats.WorkEnd.ShouldBe("2014.10.24 23:59".Date());
        }
    }
}
