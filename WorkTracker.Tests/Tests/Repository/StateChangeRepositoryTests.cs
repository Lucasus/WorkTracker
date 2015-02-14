using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using WorkTracker.Business;
using WorkTracker.Infrastructure;
using WorkTracker.Repositories;

namespace WorkTracker.Tests
{
    [TestClass]
    public class StateChangeRepositoryTests
    {
        [TestMethod]
        public void Should_Return_Correct_Records_By_Date()
        {
            // arrange
            var dataProvider = Substitute.For<IStringDataProvider>();
            dataProvider.GetAll().Returns(new List<string>()
            {
                   "V1.0,Work,2014-10-23 23:00:00",
                "V1.0,Stopped,2014-10-24 00:15:15",
                   "V1.0,Work,2014-10-24 06:00:00",
                "V1.0,Stopped,2014-10-24 12:00:00",
                   "V1.0,Work,2014-10-24 23:30:00",
                "V1.0,Stopped,2014-10-25 00:05:00"
            });

            // act
            var oneDayChanges = new StateChangeRepository(dataProvider).GetByDate("2014.10.24 12:00".Date());

            // assert
            oneDayChanges.Select(x => new { State = x.StateName, Date = x.ChangeDate }).ShouldBe(new[] 
            { 
                new { State = StateNamesEnum.Stopped, Date = "2014.10.24 00:15:15".Date() }, 
                new { State = StateNamesEnum.Work, Date = "2014.10.24  6:00".Date() }, 
                new { State = StateNamesEnum.Stopped, Date = "2014.10.24 12:00".Date() }, 
                new { State = StateNamesEnum.Work, Date = "2014.10.24 23:30".Date() }, 
            }.ToList());
        }
    }
}
