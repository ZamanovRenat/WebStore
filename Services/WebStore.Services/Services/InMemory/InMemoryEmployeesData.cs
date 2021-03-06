using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services.InMemory
{
    [Obsolete("Поддержка класса размещения товаров в памяти прекращена", true)]
    public class InMemoryEmployeesData : IEmployeesData
    {
        //[Route]

        private int _CurrentMaxId;
        //private static object employee;
        ////Проверка есть ли такой сотрудник, если нет выбрасывает исключение
        //private static void CheckedNull()
        //{
        //    if (employee is null) throw new ArgumentNullException(nameof(employee));

        //}
        public InMemoryEmployeesData()
        {
            _CurrentMaxId = TestData.Employees.Max(i => i.Id);
        }
        public IEnumerable<Employee> GetAll()
        {
            return TestData.Employees;
        }

        public Employee Get(int id)
        {
            return TestData.Employees.SingleOrDefault(employee => employee.Id == id);
        }
        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (TestData.Employees.Contains(employee)) return employee.Id; //Для БД не нужно
            employee.Id = ++_CurrentMaxId;
            TestData.Employees.Add(employee);

            return employee.Id;
        }
        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Contains(employee)) return; //Для БД не нужно

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
            return TestData.Employees.Remove(db_item);
        }
    }
}
