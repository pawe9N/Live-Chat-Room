using NUnit.Framework;
using LiveChatRoom.Controllers;
using System;
using System.Web.Mvc;

namespace Tests
{
    [TestFixture]
    public class UserControllerNonActionTests
    {
        [Test]
        public void IsUserAdult_IsBornToday_ReturnsFalse()
        {
            UserController userController = new UserController();
            DateTime? today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var result = userController.IsUserAdult(today);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsUserAdult_IsAdult_ReturnsTrue()
        {
            UserController userController = new UserController();
            DateTime? today = new DateTime(1997, 9, 9);

            var result = userController.IsUserAdult(today);

            Assert.IsTrue(result);
        }
    }
}
