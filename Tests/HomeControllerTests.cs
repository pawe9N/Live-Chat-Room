using LiveChatRoom;
using LiveChatRoom.Controllers;
using LiveChatRoom.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Account_WhenUserAuthorized_ActionResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            mockUser.Setup(m => m.Users).Returns(new User[]
                {
                    new User{ UserName="John", Password=Crypto.Hash("123456789"), EmailID="john@wp.pl", DateOfBirth=DateTime.Now.AddYears(-20), Gender="Male", IsEmailVerified=true }
                }.AsQueryable());
            HomeController controller = new HomeController(mockUser.Object);

            var actual = controller.Account("john@wp.pl");

            Assert.IsInstanceOfType(actual, typeof(ActionResult));
        }

        [TestMethod]
        public void Account_WhenUserNoAuthorized_ActionResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            HomeController controller = new HomeController(mockUser.Object);

            var actual = controller.Account("");

            Assert.IsInstanceOfType(actual, typeof(ActionResult));
        }

        [TestMethod]
        public void ActiveUsers_Returns_ActionResult()
        {
            HomeController controller = new HomeController();

            var actual = controller.ActiveUsers();

            Assert.IsInstanceOfType(actual, typeof(ActionResult));
        }

        [TestMethod]
        public void Chat_WhenUserNoAuthorized_Throws()
        {
            HomeController controller = new HomeController();

            var exception = Assert.ThrowsException<InvalidOperationException>(() => controller.Chat());

            StringAssert.Contains(exception.Message, "No connection string");
        }

        [TestMethod]
        public void Chat_WhenUserAuthorized_ReturnActionResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            mockUser.Setup(m => m.Users).Returns(new User[]
                {
                    new User{ UserName="John", Password=Crypto.Hash("123456789"), EmailID="john@wp.pl", DateOfBirth=DateTime.Now.AddYears(-20), Gender="Male", IsEmailVerified=true }
                }.AsQueryable());
            HomeController controller = new HomeController(mockUser.Object);

            var actual = controller.Chat("john@wp.pl");

            Assert.IsInstanceOfType(actual, typeof(ActionResult));
        }
    }
}
