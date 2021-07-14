using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient Client) : base(Client, WebAPIAddress.Products) { }

        public IEnumerable<Section> GetSections() => Get<IEnumerable<SectionDTO>>($"{Address}/sections").FromDTO();

        public Section GetSection(int id) => Get<SectionDTO>($"{Address}/sections/{id}").FromDTO();

        public IEnumerable<Brand> GetBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/brands").FromDTO();

        public Brand GetBrand(int id) => Get<BrandDTO>($"{Address}/brands/{id}").FromDTO();

        public ProductsPage GetProducts(ProductFilter Filter = null)
        {
            var response = Post(Address, Filter ?? new ProductFilter());
            var products = response.Content.ReadFromJsonAsync<ProductsPageDTO>().Result;
            return products.FromDTO();
        }

        public Product GetProductById(int Id) => Get<ProductDTO>($"{Address}/{Id}").FromDTO();
        public int Add(Product product)
        {
            var response = Post(Address, product);
            var id = response.Content.ReadFromJsonAsync<int>().Result;
            return id;
        }

        public void Update(Product product)
        {
            Put(Address, product);
        }

        public bool Delete(int id)
        {
            var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
            return result;
        }
    }
}
