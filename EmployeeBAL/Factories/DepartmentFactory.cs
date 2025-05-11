using EmployeeBAL.Departments;
using EmployeeBAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBAL.Factories
{
    public class DepartmentFactory
    {
        public static IDepartmentOvertimeCalculator? GetCalculator(string departmentName)
        {
            return departmentName switch
            {
                "IT" => new ITDepartment(),
                "Sales" => new SalesDepartment(),
                "HR" => new HRDepartment(),
                "OnSite" => new OnSiteDepartment(),
                _ => null
            };
        }
    }
}
