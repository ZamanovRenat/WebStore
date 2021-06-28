using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {

        private readonly ILogger<InMemoryProductData> _Logger;
        private int _CurrentMaxId;

        public InMemoryProductData(ILogger<InMemoryProductData> Logger)
        {
            _Logger = Logger;
            _CurrentMaxId = TestData.Products.Max(i => i.Id);
        }
        public IEnumerable<Brand> GetBrands()
        {
            return TestData.Brands;
        }
        public IEnumerable<Section> GetSections()
        {
            return TestData.Sections;
        }

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (Filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);

            if(Filter?.BrandId is { } brand_id)
                query = query.Where(product => product.BrandId == brand_id);
            
            return query;
        }

        public Product GetProductById(int Id)
        {
            return TestData.Products.FirstOrDefault(p => p.Id == Id);
        }

        public int Add(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            if (TestData.Products.Contains(product)) return product.Id; // характерно только если inMemory!!!! Для БД не нужно!

            product.Id = ++_CurrentMaxId;
            TestData.Products.ToList().Add(product);

            _Logger.LogInformation("Товар id:{0} добавлен", product.Id);

            return product.Id;

        }

        public void Update(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            if (TestData.Products.Contains(product)) return; // характерно только если inMemory!!!! Для БД не нужно!

            var db_item = GetProductById(product.Id);
            if (db_item is null) return;
            db_item.Name = product.Name;
            db_item.Price = product.Price;
            db_item.ImageUrl = product.ImageUrl;
            db_item.Section = product.Section;
            db_item.SectionId = product.SectionId;
            db_item.Brand = product.Brand;
            db_item.BrandId = product.BrandId;
            db_item.Order = product.Order;

            _Logger.LogInformation("Товар id:{0} отредактирован", product.Id);
        }

        public bool Delete(int id)
        {
            var db_item = GetProductById(id);
            if (db_item is null)
            {
                _Logger.LogWarning("При удалении товар id:{0} не найден", id);
                return false;
            }

            var result = TestData.Products.ToList().Remove(db_item);

            if (result)
                _Logger.LogInformation("Товар id:{0} удалён", id);
            else
                _Logger.LogError("При удалении товар id:{0} не найден", id);

            return result;
        }
    }
}
