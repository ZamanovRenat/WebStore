using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections;

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.Ids?.Length > 0)
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            else
            {
                if (Filter?.SectionId is { } section_id)
                    query = query.Where(product => product.SectionId == section_id);

                if (Filter?.BrandId is { } brand_id)
                    query = query.Where(product => product.BrandId == brand_id);
            }

            return query;
        }

        public Product GetProductById(int Id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .SingleOrDefault(p => p.Id == Id);

        public int Add(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            _db.Add(product);

            _db.SaveChanges();

            return product.Id;
        }

        public void Update(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            _db.Update(product);

            _db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var product = _db.Products
                .Select(e => new Product { Id = e.Id })
                .FirstOrDefault(e => e.Id == id);
            if (product is null) return false;

            _db.Remove(product);

            _db.SaveChanges();

            return true;
        }
    }
}
