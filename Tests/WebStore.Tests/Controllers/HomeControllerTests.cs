using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void About_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.About();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Account_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.Account();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Checkout_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.Checkout();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Contact_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.Contact();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void MyAccount_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.MyAccount();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void ProductDetails_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.ProductDetails();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Shop_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.Shop();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void ShopList_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.ShopList();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void WishList_Returns_View()
        {
            var configurationMock = new Mock<IConfiguration>();

            var controller = new HomeController(configurationMock.Object);

            var result = controller.WishList();

            Assert.IsType<ViewResult>(result);
        }
    }
}

