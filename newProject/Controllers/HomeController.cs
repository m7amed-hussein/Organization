using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newProject.Models;
using newProject.ViewModels;

namespace newProject.Controllers
{
    
    public class HomeController:Controller
    {
        private readonly IEmployeeRepository _employeeRepository;


        public HomeController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        
        public ViewResult Details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee= employee
            };
            
            return View(homeDetailsViewModel);
        }
        
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try {
                    var e= _employeeRepository.Add(employee);
                    return RedirectToAction("Details",e.Id);

                }
                catch(Exception e)
                {
                    return View();
                }
               
            }
            return View(employee);
        }


    }
}
