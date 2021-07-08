using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void Details_Returns_with_Correct_View()
        {
            #region Arrange
            const decimal expectedPrice = 10m;
            const int expectedId = 1;
            const string expectedName = "Product 1";

            var productDataMock = new Mock<IProductData>();
            productDataMock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns<int>(id => new Product
               {
                   Id = id,
                   Name = $"Product {id}",
                   Order = 1,
                   Price = expectedPrice,
                   ImageUrl = $"img_{id}.png",
                   Brand = new Brand { Id = 1, Name = "Brand", Order = 1 },
                   Section = new Section { Id = 1, Order = 1, Name = "Section" }
               });

            var controller = new CatalogController(productDataMock.Object);

            #endregion

            #region Act

            var result = controller.Details(expectedId);

            #endregion

            #region Assert

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);

            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedPrice, model.Price);

            productDataMock.Verify(s => s.GetProductById(It.IsAny<int>()));
            productDataMock.VerifyNoOtherCalls();

            #endregion
        }
    }
}
