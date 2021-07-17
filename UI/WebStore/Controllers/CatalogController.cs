using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string _PageSizeConfigName = "CatalogPageSize";

        private readonly IProductData _ProductData;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductData ProductData, IConfiguration configuration)
        {
            _ProductData = ProductData;
            _configuration = configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int Page =1, int? PageSize = null)
        {
            var page_size = PageSize
                            ?? (int.TryParse(_configuration["CatalogPageSize"], out var value) ? value : null);
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size,
            };

            var (products, totalCount) = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView(),
                PageViewModel = new PageViewModel
                {
                    Page = Page,
                    PageSize = page_size ?? 0,
                    TotalItems = totalCount,
                },
            });
        }

        public IActionResult Details(int Id)
        {
            var product = _ProductData.GetProductById(Id);
            if (product is null)
                return NotFound();

            return View(product.ToView());
        }
        public IActionResult GetFeaturesItems(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null) =>
            PartialView("Partial/_Products", GetProducts(BrandId, SectionId, Page, PageSize));

        private IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize) =>
            _ProductData.GetProducts(
                new ProductFilter
                {
                    BrandId = BrandId,
                    SectionId = SectionId,
                    Page = Page,
                    PageSize = PageSize ?? _configuration.GetValue(_PageSizeConfigName, 6),
                }).Products.OrderBy(p => p.Order).ToView();
    }
}