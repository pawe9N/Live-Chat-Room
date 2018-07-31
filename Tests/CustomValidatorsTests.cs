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

        [TestMethod]
        public void IsImageUrl_ValidImageURL_ReturnsTrue()
        {
            string url = "https://www.at-languagesolutions.com/en/wp-content/uploads/2016/06/http-1.jpg";

            var result = IsImageUrlAttribute.IsImageUrl(url);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsImageUrl_NotValidImageURL_ReturnsTrue()
        {
            string url = "google.com";

            var result = IsImageUrlAttribute.IsImageUrl(url);

            Assert.IsFalse(result);
        }
    }
}
