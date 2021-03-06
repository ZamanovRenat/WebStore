using Microsoft.AspNetCore.Http;

namespace WebStore.Domain.ViewModels
{
    
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Section { get; set; }
        public int SectionId { get; set; }

        public string Brand { get; set; }
        public int BrandId { get; set; }
        public int Order { get; set; }
        public IFormFile Image { get; set; }

    }
}