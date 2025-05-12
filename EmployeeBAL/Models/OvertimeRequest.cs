using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBAL.Models
{
    public class OvertimeRequest
    {
        public int EmployeeId { get; set; }
        public int Hours { get; set; }
    }
}
