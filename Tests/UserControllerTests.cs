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
using System.Web.Mvc;

namespace Tests
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void Registration_Returns_ViewResult()
        {
            UserController controller = new UserController();

            var actual = controller.Registration();

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void VerifyAccount_NoId_ReturnHttpNotFoundResult()
        {
            UserController controller = new UserController();

            var actual = controller.VerifyAccount(null);

            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void VerifyAccount_ValidID_ReturnsViewResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            Guid guid = Guid.NewGuid();
            mockUser.Setup(m => m.Users).Returns(new User[]
                {
                    new User{
                        UserName ="John",
                        Password =Crypto.Hash("123456789"),
                        EmailID ="john@wp.pl",
                        DateOfBirth =DateTime.Now.AddYears(-20),
                        Gender ="Male",
                        IsEmailVerified =true,
                        ActivationCode = guid}
                }.AsQueryable());
            UserController controller = new UserController(mockUser.Object);

            var actual = controller.VerifyAccount(guid.ToString());

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void Login_Returns_ViewResult()
        {
            UserController controller = new UserController();

            var actual = controller.Login();

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void ForgotPassword_Returns_ViewResult()
        {
            UserController controller = new UserController();

            var actual = controller.ForgotPassword();

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void ResetPassword_NoId_ReturnsHttpNotFoundResult()
        {
            UserController controller = new UserController();

            var actual = controller.ResetPassword(null);

            Assert.IsInstanceOfType(actual, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void ResetPassword_ValidId_ReturnsViewResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            Guid guid = Guid.NewGuid();
            mockUser.Setup(m => m.Users).Returns(new User[]
                {
                    new User{
                        UserName ="John",
                        Password =Crypto.Hash("123456789"),
                        EmailID ="john@wp.pl",
                        DateOfBirth =DateTime.Now.AddYears(-20),
                        Gender ="Male",
                        IsEmailVerified =true,
                        ResetPasswordCode = guid.ToString()}
                }.AsQueryable());
            UserController controller = new UserController(mockUser.Object);

            var actual = controller.ResetPassword(guid.ToString());

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void PostRegistration_SendNull_Throws()
        {
            UserController controller = new UserController();

            var actual = Assert.ThrowsException<NullReferenceException>(() => controller.Registration(null));
        }

        [TestMethod]
        public void PostLogin_SendNull_Throws()
        {
            UserController controller = new UserController();

            var actual = Assert.ThrowsException<InvalidOperationException>(() => controller.Login(null));
        }

        [TestMethod]
        public void PostLogin_LoginWithValidUser_ReturnsViewResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            mockUser.Setup(m => m.Users).Returns(new User[]
                {
                    new User{ UserName="John", Password=Crypto.Hash("123456789"), EmailID="john@wp.pl", DateOfBirth=DateTime.Now.AddYears(-20), Gender="Male", IsEmailVerified=true }
                }.AsQueryable());
            UserController controller = new UserController(mockUser.Object);
            UserLoginModel userLogin = new UserLoginModel { EmailId = "john@wp.pl", Password = Crypto.Hash("123456789"), RememberMe = false };

            var actual = controller.Login(userLogin);

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }

        [TestMethod]
        public void Logout_Returns_ActionResult()
        {
            UserController controller = new UserController();

            var actual = controller.Logout();

            Assert.IsInstanceOfType(actual, typeof(ActionResult));
        }

        [TestMethod]
        public void PostForgotPassword_SendNull_Throws()
        {
            UserController controller = new UserController();

            var actual = Assert.ThrowsException<InvalidOperationException>(() => controller.ForgotPassword(null));
        }

        [TestMethod]
        public void PostResetPassword_SendNull_Throws()
        {
            UserController controller = new UserController();

            var actual = Assert.ThrowsException<InvalidOperationException>(() => controller.GetResetPassword(null));
        }

        [TestMethod]
        public void PostResetPassword_GetValidModel_ReturnViewResult()
        {
            Mock<IUserRepository> mockUser = new Mock<IUserRepository>();
            Guid guid = Guid.NewGuid();
            mockUser.Setup(m => m.Users).Returns(new User[]
                {
                    new User{
                        UserName ="John",
                        Password =Crypto.Hash("123456789"),
                        EmailID ="john@wp.pl",
                        DateOfBirth =DateTime.Now.AddYears(-20),
                        Gender ="Male",
                        IsEmailVerified =true,
                        ResetPasswordCode = guid.ToString()}
                }.AsQueryable());
            UserController controller = new UserController(mockUser.Object);
            ResetPasswordModel model = new ResetPasswordModel { NewPassword = "1234567", ConfirmPassword = "1234567", ResetCode = guid.ToString() };

            var actual = controller.GetResetPassword(model);

            Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }
    }
}
