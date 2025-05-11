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
            var emp = _repo.GetById(id);
            _logger.Log($"Fetched employee with ID {id}");
            return emp == null ? NotFound() : Ok(emp);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _repo.GetAll();
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

        // 🔸 New API for Overtime Calculation using Abstract Factory
        [HttpPost("overtime")]
        public IActionResult GetOvertimePay([FromBody] OvertimeRequest request)
        {
            var department = _deptRepo.GetById(request.DepartmentId);

            if (department == null)
                return NotFound("Department not found");

            var departmentName = department.DepartmentName;
            var factory = departmentName switch
            {
                "IT" or "HR" => (IOvertimeFactory)_indoorFactory,
                "Sales" or "OnSite" => (IOvertimeFactory)_outdoorFactory,
                _ => null
            };


            if (factory == null)
                return BadRequest("Unknown department category");

            var calculator = factory.GetOvertimeCalculator(departmentName);
            var pay = calculator.CalculateOvertime(request.Hours);

            _logger.Log($"Calculated overtime for Employee {request.EmployeeId}, Dept: {departmentName}, Hours: {request.Hours} => Pay: Rs. {pay}");
            return Ok(new { OvertimePay = pay });
        }
    }
}
