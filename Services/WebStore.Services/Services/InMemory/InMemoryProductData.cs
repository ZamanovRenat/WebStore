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
    [Obsolete("Поддержка класса размещения товаров в памяти прекращена", true)]
    public class InMemoryProductData : IProductData
    {
        private readonly ILogger<InMemoryProductData> _Logger;
        private int _CurrentMaxId;

        public InMemoryProductData(ILogger<InMemoryProductData> Logger)
        {
            _Logger = Logger;
            _CurrentMaxId = TestData.Products.Max(i => i.Id);
        }
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public Section GetSection(int id) => throw new NotSupportedException();

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public Brand GetBrand(int id) => throw new NotSupportedException();

        public ProductsPage GetProducts(ProductFilter Filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            //if(Filter?.SectionId != null)
            //    query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);

            if (Filter?.BrandId is { } brand_id)
                query = query.Where(product => product.BrandId == brand_id);

            var total_count = query.Count();

            if (Filter is { PageSize: > 0 and var page_size, Page: > 0 and var page_number })
                query = query
                    .Skip((page_number - 1) * page_size)
                    .Take(page_size);

            return new ProductsPage(query, total_count);
        }

        public Product GetProductById(int Id) => TestData.Products.SingleOrDefault(p => p.Id == Id);
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