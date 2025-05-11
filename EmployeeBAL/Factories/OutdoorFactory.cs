using EmployeeBAL.Departments;
using EmployeeBAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBAL.Factories
{
    public class OutdoorFactory : IOvertimeFactory
    {
        public IDepartmentOvertimeCalculator GetOvertimeCalculator(string departmentName)
        {
            return departmentName switch
            {
                "Sales" => new SalesDepartment(),
                "OnSite" => new OnSiteDepartment(),
                _ => throw new Exception("Invalid outdoor department")
            };
        }
    }
}
