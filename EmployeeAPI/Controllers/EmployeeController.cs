using EmployeeDAL.Services;
using EmployeeDAL.Models;
using EmployeeDAL.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using EmployeeBAL.Factories;
using EmployeeBAL.Models;
using EmployeeBAL.Interfaces;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _repo;
        private readonly DepartmentRepository _deptRepo;
        private readonly ILoggerService _logger;
        private readonly IndoorFactory _indoorFactory;
        private readonly OutdoorFactory _outdoorFactory;

        public EmployeeController(
            EmployeeRepository repo,
            DepartmentRepository deptRepo,
            ILoggerService logger,
            IndoorFactory indoorFactory,
            OutdoorFactory outdoorFactory)
        {
            _repo = repo;
            _deptRepo = deptRepo;
            _logger = logger;
            _indoorFactory = indoorFactory;
            _outdoorFactory = outdoorFactory;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Employee emp = _repo.GetById(id);
            _logger.Log($"Fetched employee with ID {id}");
            return emp == null ? NotFound() : Ok(emp);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Employee> employees = _repo.GetAll();
            _logger.Log("Fetched all employees");
            return Ok(employees);
        }

        [HttpPost]
        public IActionResult Create(AddEmployeeDTO emp)
        {
            _repo.Create(emp);
            _logger.Log($"Created employee {emp.EmployeeName}");
            return Ok("Employee added");
        }

        [HttpPut]
        public IActionResult Update(Employee emp)
        {
            _repo.Update(emp);
            _logger.Log($"Updated employee ID {emp.EmployeeId}");
            return Ok("Employee updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.SoftDelete(id);
            _logger.Log($"Soft deleted employee ID {id}");
            return Ok("Employee soft deleted");
        }

        [HttpPost("overtime/factory")]
        public IActionResult GetOvertimePayUsingFactory([FromBody] OvertimeRequest request)
        {
            Employee employee = _repo.GetById(request.EmployeeId);
            if (employee == null)
                return NotFound("Employee not found");

            Department department = _deptRepo.GetById(employee.DepartmentId);
            if (department == null)
                return NotFound("Department not found");

            IDepartmentOvertimeCalculator calculator = DepartmentFactory.GetCalculator(department.DepartmentName); // Factory pattern
            if (calculator == null)
                return BadRequest("Unsupported department");

            decimal pay = calculator.CalculateOvertime(request.Hours);
            _logger.Log($"[Factory] Employee {employee.EmployeeName} (Dept: {department.DepartmentName}) => Rs. {pay}");
            return Ok(new { OvertimePay = pay });
        }

        [HttpPost("overtime/abstractfactory")]
        public IActionResult GetOvertimePayUsingAbstractFactory([FromBody] OvertimeRequest request)
        {
            Employee employee = _repo.GetById(request.EmployeeId);
            if (employee == null)
                return NotFound("Employee not found");

            Department department = _deptRepo.GetById(employee.DepartmentId);
            if (department == null)
                return NotFound("Department not found");

            string departmentName = department.DepartmentName;

            IOvertimeFactory? factory = departmentName switch
            {
                "IT" or "HR" => _indoorFactory,
                "Sales" or "OnSite" => _outdoorFactory,
                _ => null
            };

            if (factory == null)
                return BadRequest("Unsupported department");

            IDepartmentOvertimeCalculator calculator = factory.GetOvertimeCalculator(departmentName);
            decimal pay = calculator.CalculateOvertime(request.Hours);

            _logger.Log($"[AbstractFactory] Employee {employee.EmployeeName} (Dept: {departmentName}) => Rs. {pay}");
            return Ok(new { OvertimePay = pay });
        }

    }
}
