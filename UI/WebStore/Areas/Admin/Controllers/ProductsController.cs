using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductData ProductData, ILogger<ProductsController> Logger)
        {
            _ProductData = ProductData;
            _logger = Logger;
        }

        public IActionResult Index() => View(_ProductData.GetProducts());
        public IActionResult Create() => View("Edit", new ProductViewModel());

        public IActionResult Edit(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null)
                return NotFound();

            var view_model = new ProductViewModel()
            {
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                SectionId = product.SectionId,
                //BrandId = product.BrandId,

            };

            return View(view_model);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel Model)
        {
            _logger.LogInformation("Редактирование товара id: {0}", Model.Id);

            Product product = new Product
            {
                Name = Model.Name,
                Price = Model.Price,
                ImageUrl = Model.Image.FileName.ToString(),
                Order = Model.Order,
                SectionId = Model.SectionId,
                BrandId = Model.BrandId,
            };

            if (Model.Image != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(Model.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int) Model.Image.Length);
                }

                product.ImageUrl = Model.Image.FileName.ToString();
            }

            if (product.Id == 0)
                _ProductData.Add(product);
            else 
                _ProductData.Update(product);

            _logger.LogInformation("Редактирование товара id:{0} завершено", Model.Id);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var product = _ProductData.GetProductById(id);
            if (product is null)
                return NotFound();

            return View(new ProductViewModel()
            {
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                SectionId = product.SectionId,
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation("Удаление товара id:{0}", id);

            _ProductData.Delete(id);

            _logger.LogInformation("Удаление товара id:{0} завершено", id);
            return RedirectToAction("Index");
        }

    }
}
