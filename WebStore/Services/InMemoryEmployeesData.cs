using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        //[Route]
        private readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
        };
        private int _CurrentMaxId;
        //private static object employee;
        ////Проверка есть ли такой сотрудник, если нет выбрасывает исключение
        //private static void CheckedNull()
        //{
        //    if (employee is null) throw new ArgumentNullException(nameof(employee));

        //}
        public InMemoryEmployeesData()
        {
            _CurrentMaxId = __Employees.Max(i => i.Id);
        }
        public IEnumerable<Employee> GetAll()
        {
            return __Employees;
        }

        public Employee Get(int id)
        {
            return __Employees.SingleOrDefault(employee => employee.Id == id);
        }
        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (__Employees.Contains(employee)) return employee.Id; //Для БД не нужно
            employee.Id = ++_CurrentMaxId;
            __Employees.Add(employee);

            return employee.Id;
        }
        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (__Employees.Contains(employee)) return; //Для БД не нужно

            var db_item = Get(employee.Id);
            if (db_item is null) return;

            //Редактирование
            db_item.LastName = employee.LastName;
            db_item.FirstName = employee.FirstName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;
        }
        public bool Delete(int id)
        {
            var db_item = Get(id);
            if (db_item is null) return false;
            return __Employees.Remove(db_item);
        }
    }
}
