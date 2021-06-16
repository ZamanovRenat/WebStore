using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
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
            throw new System.NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
