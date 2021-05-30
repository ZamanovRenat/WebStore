using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        
        public IActionResult Index()
        {
            return View(__Employees);
        }

        public IActionResult Details(int id)
        {
            //Получение сотрудника по id
            var employee = __Employees.FirstOrDefault(a => a.Id.Equals(id));
            //Проверка есть ли такой сотрудник
            if (employee is null)
                return NotFound();

            return View(employee);
        }
        /// <summary>
        /// Редактирование сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IActionResult Edit(int? id)
        {
            Employee model;
            if (id.HasValue)
            {
                model = __Employees.FirstOrDefault(a => a.Id.Equals(id));
                if (model is null)
                    return NotFound(); // возвращаем результат 404 Not Found
            }
            else
            {
                model = new Employee();
            }
            return View(model);
        }

    }
}
