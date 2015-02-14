using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WorkTracker.Business;
using WorkTracker.Infrastructure;
using WorkTracker.Repositories;
using WorkTracker.Tests.Infrastructure;
using Shouldly;
using System;
using System.Linq;

namespace WorkTracker.Tests
{
    [TestClass]
    public class StatsServiceTests
    {
        [TestMethod]
        public void Should_Return_Correct_RealTime_Today_Stats_When_Day_Changes()
        {
            // arrange
            var stateChangeRepository = Substitute.For<IStateChangeRepository>();

            var firstDayDate = "2014.10.24 23:35".Date();
            stateChangeRepository.GetByDate(firstDayDate).Returns(DataBuilder.StateChangesCollection(
                    StateNamesEnum.Work.On("2014.10.24  6:00"),
                    StateNamesEnum.Stopped.On("2014.10.24 12:00"),
                    StateNamesEnum.Work.On("2014.10.24 23:30")
                ));

            var secondDayDate = "2014.10.25 14:00".Date();
            stateChangeRepository.GetByDate(secondDayDate).Returns(DataBuilder.StateChangesCollection(
                    StateNamesEnum.Stopped.On("2014.10.25  1:30"),
                    StateNamesEnum.Work.On("2014.10.25 12:30"),
                    StateNamesEnum.Stopped.On("2014.10.25 13:50")
                ));

            var timeProvider = Substitute.For<ITimeProvider>();
            var statsService = new StatsService(Substitute.For<IDailyStatsRepository>(), stateChangeRepository, new DailyStatsCalculator(), new GlobalStatsCalculator(), timeProvider);

            // act
            timeProvider.CurrentDate.Returns(firstDayDate);
            var fistDayStats = statsService.GetRealTimeTodayStats();

            timeProvider.CurrentDate.Returns(secondDayDate);
            var secondDayStats = statsService.GetRealTimeTodayStats();

            // assert
            fistDayStats.WorkTime.ShouldBe(new TimeSpan(6, 5, 0));
            secondDayStats.WorkTime.ShouldBe(new TimeSpan(2, 50, 0));
        }
    }
}
