using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WorkTracker.Repositories;
using WorkTracker.Infrastructure;
using System.Collections.Generic;
using WorkTracker.Business;

namespace WorkTracker.Tests
{
    [TestClass]
    public class StateChangeRepositoryTests
    {
        [TestMethod]
        public void ShouldReturnCorrectList()
        {
            var todayDate = new DateTime(2014, 10, 24);
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


            var repository = new StateChangeRepository(dataProvider);

            var oneDayChanges = repository.GetByDate(todayDate);

            Assert.AreEqual(4, oneDayChanges.Count);

            Assert.AreEqual(StateNamesEnum.Stopped, oneDayChanges[0].StateName);
            Assert.AreEqual(todayDate.Date.AddMinutes(15).AddSeconds(15), oneDayChanges[0].ChangeDate);

            Assert.AreEqual(StateNamesEnum.Work, oneDayChanges[1].StateName);
            Assert.AreEqual(todayDate.Date.AddHours(6), oneDayChanges[1].ChangeDate);

            Assert.AreEqual(StateNamesEnum.Stopped, oneDayChanges[2].StateName);
            Assert.AreEqual(todayDate.Date.AddHours(12), oneDayChanges[2].ChangeDate);

            Assert.AreEqual(StateNamesEnum.Work, oneDayChanges[3].StateName);
            Assert.AreEqual(todayDate.Date.AddHours(23).AddMinutes(30), oneDayChanges[3].ChangeDate);
        }
    }
}
