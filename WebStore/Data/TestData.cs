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
        };
    }
}
