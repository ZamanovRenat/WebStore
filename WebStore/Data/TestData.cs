using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
        };

        public static IEnumerable<Section> Sections { get; } = new[]
        {
            new Section {Id = 1, Name = "Phones", Order = 0},
            new Section {Id = 2, Name = "Cameras", Order = 1},
            new Section {Id = 3, Name = "TV&Audio", Order = 2},
            new Section {Id = 4, Name = "Tablets", Order = 3, ParentId = 1},
            new Section {Id = 5, Name = "Computers", Order = 4},
            new Section {Id = 6, Name = "Watches", Order = 5, ParentId = 1},
        };

        public static IEnumerable<Brand> Brands { get; } = new[]
        {
            new Brand {Id = 1, Name = "Apple", Order = 0 },
            new Brand {Id = 2, Name = "Samsung", Order = 1 },
            new Brand {Id = 3, Name = "Sony", Order = 2 },
            new Brand {Id = 4, Name = "Xiaomi", Order = 3 },
        };
        public static IEnumerable<Product> Products { get; } = new[]
        {
            new Product { Id = 1, Name = "IPhone 12", Price = 1025, ImageUrl = "~/img/product/IPhone12.jpg", Order = 0, SectionId = 1, BrandId = 1 },
            new Product { Id = 2, Name = "Sony HD", Price = 1025, ImageUrl = "~/img/product/Sony.jpg", Order = 1, SectionId = 2, BrandId = 3 },
            new Product { Id = 3, Name = "XIAOMI Mi TV", Price = 1025, ImageUrl = "~/img/product/Xiaomi.jpg", Order = 2, SectionId = 3, BrandId = 4 },
            new Product { Id = 4, Name = "Apple iMac", Price = 1025, ImageUrl = "~/img/product/iMac.jpg", Order = 3, SectionId = 5, BrandId = 1 },
            new Product { Id = 5, Name = "Samsung Watch", Price = 1025, ImageUrl = "~/img/product/Watch.jpg", Order = 4, SectionId = 6, BrandId = 2 },
            new Product { Id = 5, Name = "Apple iPad", Price = 1025, ImageUrl = "~/img/product/iPad.jpg", Order = 5, SectionId = 4, BrandId = 1 },
        };
    }
}
