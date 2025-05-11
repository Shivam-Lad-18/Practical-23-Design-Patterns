using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBAL.Interfaces
{
    public interface IDepartmentOvertimeCalculator
    {
        decimal CalculateOvertime(int hours);
    }
}
