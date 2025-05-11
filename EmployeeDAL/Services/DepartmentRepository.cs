using EmployeeDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAL.Services
{
    public class DepartmentRepository
    {
        private static readonly List<Department> departments = new List<Department>
        {
            new Department { DepartmentId = 1, DepartmentName = "IT" },
            new Department { DepartmentId = 2, DepartmentName = "Sales" },
            new Department { DepartmentId = 3, DepartmentName = "HR" },
            new Department { DepartmentId = 4, DepartmentName = "OnSite" }
        };

        public Department? GetById(int id)
        {
            return departments.FirstOrDefault(d => d.DepartmentId == id);
        }

        public List<Department> GetAll()
        {
            return departments;
        }
    }
}
