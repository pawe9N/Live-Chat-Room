using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static LiveChatRoom.App_Classes.CustomValidators;

namespace Tests
{
    [TestClass]
    public class CustomValidatorsTests
    {
        [TestMethod]
        public void IsUserAdult_IsBornToday_ReturnsFalse()
        {
            DateTime? today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var result = DateOfBirthAdultAttribute.IsUserAdult(today);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUserAdult_IsAdult_ReturnsTrue()
        {
            DateTime? date = new DateTime(1997, 9, 9);

            var result = DateOfBirthAdultAttribute.IsUserAdult(date);

            Assert.IsTrue(result);
        }
    }
}
