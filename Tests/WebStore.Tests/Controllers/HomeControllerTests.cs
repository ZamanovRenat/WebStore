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
    }
}
