using EmployeeBAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBAL.Departments
{
    public class HRDepartment : IDepartmentOvertimeCalculator
    {
        public decimal CalculateOvertime(int hours) => hours * 150;
    }
}
