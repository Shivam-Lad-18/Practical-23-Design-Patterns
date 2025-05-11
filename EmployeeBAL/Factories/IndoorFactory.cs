using EmployeeBAL.Departments;
using EmployeeBAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBAL.Factories
{
    public class IndoorFactory : IOvertimeFactory
    {
        public IDepartmentOvertimeCalculator GetOvertimeCalculator(string departmentName)
        {
            return departmentName switch
            {
                "IT" => new ITDepartment(),
                "HR" => new HRDepartment(),
                _ => throw new Exception("Invalid indoor department")
            };
        }
    }
}
