using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public async Task CheckOut_ModelState_Invalid_Returns_View_with_Model()
        {
            const string expectedName = "Test order";

            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            var controller = new CartController(cartServiceMock.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            var orderModel = new OrderViewModel
            {
                Name = expectedName
            };

            var result = await controller.CheckOut(orderModel, orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);

            Assert.Equal(expectedName, model.Order.Name);
        }

        [TestMethod]
        public async Task CheckOut_ModelState_Valid_Call_Service_and_Returns_Redirect()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
               .Setup(c => c.GetViewModel())
               .Returns(new CartViewModel
               {
                   Items = new[] { (new ProductViewModel { Name = "Test product" }, 1) }
               });

            const int expectedOrderId = 1;
            const string expectedOrderName = "Test order";
            const string expectedOrderAddress = "Test address";
            const string expectedOrderPhone = "123";
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
               .Setup(c => c.CreateOrder(It.IsAny<string>(), It.IsAny<CartViewModel>(), It.IsAny<OrderViewModel>()))
               .ReturnsAsync(
                    new Order
                    {
                        Id = expectedOrderId,
                        Name = expectedOrderName,
                        Address = expectedOrderAddress,
                        Phone = expectedOrderPhone,
                        Date = DateTime.Now,
                        Items = Array.Empty<OrderItem>()
                    });

            const string expectedUser = "Test user";
            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedUser) }))
                    }
                }
            };

            var orderModel = new OrderViewModel
            {
                Name = expectedOrderName,
                Address = expectedOrderAddress,
                Phone = expectedOrderPhone
            };

            var result = await controller.CheckOut(orderModel, orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);

            Assert.Equal(expectedOrderId, redirectResult.RouteValues["id"]);
        }
    }
}
