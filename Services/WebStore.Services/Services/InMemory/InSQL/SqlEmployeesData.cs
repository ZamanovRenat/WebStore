using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InMemory.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;

        public SqlEmployeesData(WebStoreDB db) => _db = db;

        public IEnumerable<Employee> GetAll() => _db.Employees.ToArray();

        public Employee Get(int id) => _db.Employees.SingleOrDefault(employee => employee.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            _db.Add(employee);

            _db.SaveChanges();

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            _db.Update(employee);

            _db.SaveChanges();
        }

        public bool Delete(int id)
        {

            var employee = _db.Employees
               .Select(e => new Employee { Id = e.Id })
               .FirstOrDefault(e => e.Id == id);
            if (employee is null) return false;

            _db.Remove(employee);

            _db.SaveChanges();

            return true;
        }
    }
}
