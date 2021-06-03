using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData productData;

        public CatalogController(IProductData ProductData)
        {
            productData = ProductData;
        }
        public IActionResult Index(int? BrandId, int? SectionId)
        {
            return View();
        }
    }
}
