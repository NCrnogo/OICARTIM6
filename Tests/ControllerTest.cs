using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Teamy;
using Teamy.Controllers;
using Teamy.Models;

namespace Tests
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void Login()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Login() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Register()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Register() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginToIndex()
        {
            HomeController controller = new HomeController();

            Users user = new Users();
            user.Name = "test@testnigmail.com";
            user.Pwd = "123";

            ViewResult result = controller.Login(user) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
