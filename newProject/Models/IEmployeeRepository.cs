using System;
using System.Collections;
using System.Collections.Generic;

namespace newProject.Models
{
    public interface IEmployeeRepository
    {
        public Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployee();
        public Employee Delete(int id);
        public Employee Update(Employee
            employee);
        public Employee Add(Employee employee);
    }
}
