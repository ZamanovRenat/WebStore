using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InMemory.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<SqlEmployeesData> _logger;

        public SqlEmployeesData(WebStoreDB db, ILogger<SqlEmployeesData> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IEnumerable<Employee> GetAll() => _db.Employees.ToArray();

        public Employee Get(int id) => _db.Employees.SingleOrDefault(employee => employee.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            _db.Add(employee);

            _db.SaveChanges();

            _logger.LogInformation($"Сотрудник {employee} добавлен");

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            _db.Update(employee);

            _db.SaveChanges();

            _logger.LogInformation($"Сотрудник {employee} отредактирован");
        }

        public bool Delete(int id)
        {

            var employee = _db.Employees
               .Select(e => new Employee { Id = e.Id })
               .FirstOrDefault(e => e.Id == id);
            if (employee is null) return false;

            _db.Remove(employee);

            _db.SaveChanges();

            _logger.LogInformation($"Сотрудник {employee} удален");

            return true;
        }
    }
}
