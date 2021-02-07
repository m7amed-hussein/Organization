using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace newProject.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext appDbContext;

        public SQLEmployeeRepository(AppDbContext appDbContext )
        {
            this.appDbContext = appDbContext;
        }

        public Employee Add(Employee employee)
        {
            Employee e = new Employee
            {
                Email = employee.Email,
                Department = employee.Department,
                Name = employee.Name
            };
            appDbContext.Add(e);
            appDbContext.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = appDbContext.Employees.Find(id);
            if (employee != null)
            {
                appDbContext.Employees.Remove(employee);
                appDbContext.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return appDbContext.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return appDbContext.Employees.Find(id);
        }

        public Employee Update(Employee employee)
        {
            var e = appDbContext.Employees.Attach(employee);
            e.State = EntityState.Modified;
            appDbContext.SaveChanges();
            return employee; 

        }
    }
}
