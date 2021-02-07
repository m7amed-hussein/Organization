using System;
using System.Collections.Generic;
using System.Linq;

namespace newProject.Models
{
    public class MockEmployeeRepository:IEmployeeRepository
    {
        private List<Employee> _employeelist;
        public MockEmployeeRepository()
        {
            _employeelist = new List<Employee>()
            {
                new Employee(){Id=1,Name="Mohamed",Department="CS",Email="someEmails1@email.com"},
                new Employee(){Id=2,Name="Mostafa",Department="HR",Email="someEmails2@email.com"},
                new Employee(){Id=3,Name="Hussein",Department="IS",Email="someEmails3@email.com"},
                new Employee(){Id=4,Name="Ibrahim",Department="SS",Email="someEmails4@email.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            _employeelist.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee e = _employeelist.FirstOrDefault(e => e.Id == id);
            if (e != null) _employeelist.Remove(e);
            return e;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeelist;
        }

        public Employee GetEmployee(int id)
        {
            return _employeelist.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee employee)
        {

            Employee e = _employeelist.FirstOrDefault(e => e.Id == employee.Id); ;
            if (e != null)
            {
                _employeelist.Remove(e);
                _employeelist.Add(employee); 
            }
            return e;
        }
    }
}
