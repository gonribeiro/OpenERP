using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.HumanResource;
using OpenERP.ViewModels.HumanResource.Departments;

namespace OpenERP.Controllers.v1.HumanResource
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/departments")]
        public async Task<List<IndexDepartmentViewModel>> GetDepartmentsAsync()
        {
            var department = await _context.Departments
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexDepartmentViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Manager = c.Manager.FullName,
                })
                .ToListAsync();

            return department;
        }

        [HttpPost("v1/departments")]
        public async Task<IActionResult> PostAsync([FromBody] DepartmentViewModel model)
        {
            try
            {
                var department = new Department
                {
                    Name = model.Name,
                    ManagerId = model.ManagerId,
                };
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    department,
                    message = "Created successfully",
                    redirectTo = $"departments/{department.Id}/edit"
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("v1/departments/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var department = await _context
                    .Departments
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (department == null)
                    return NotFound("Department not found");

                return Ok(department);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("v1/departments/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] DepartmentViewModel model)
        {
            try
            {
                var department = await _context
                    .Departments
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (department == null)
                    return NotFound("Department not found");

                department.Name = model.Name;
                department.ManagerId = model.ManagerId;

                _context.Departments.Update(department);
                await _context.SaveChangesAsync();

                return Ok(new { department, message = "Saved successfully" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("v1/departments/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var department = await _context
                    .Departments
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (department == null)
                    return NotFound("Department not found");

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    department,
                    message = "Deleted successfully",
                    redirectTo = $"departments"
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("v1/departments/employeeCount")]
        public async Task<ActionResult<List<DepartmentEmployeeCountViewModel>>> GetEmployeeCountByDepartment(DateTime? date = null)
        {
            date ??= DateTime.Today;

            var departmentEmployeeCounts = await _context.DepartmentEmployee
                .Include(de => de.Department)
                .Include(de => de.Employee)
                .Where(de => de.Employee.InactiveDate == null)
                .GroupBy(de => de.Department)
                .Select(g => new DepartmentEmployeeCountViewModel
                {
                    DepartmentName = g.Key.Name,
                    EmployeeCount = g.Count(),
                    OnVacationCount = g.Count(de => _context.Vacations
                        .Any(v => v.EmployeeId == de.Employee.Id && v.StartDate <= date && v.EndDate >= date))
                })
                .ToListAsync();

            return Ok(departmentEmployeeCounts);
        }

        [HttpGet("v1/departments/compensationByDepartment")]
        public async Task<ActionResult<List<DepartmentCompensationViewModel>>> GetEmployeeCompensationByDepartment()
        {
            var employees = await _context.Employees
                .Include(e => e.Remunerations)
                .Where(e => e.InactiveDate == null)
                .ToListAsync();

            var departmentCompensations = new Dictionary<string, decimal>();
            var departmentSharedCompensations = new Dictionary<string, decimal>();

            var employeeDepartmentCountsList = await _context.DepartmentEmployee
                .GroupBy(de => de.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    DepartmentCount = g.Count()
                })
                .ToListAsync();

            foreach (var employee in employees)
            {
                var totalCompensation = employee.Remunerations.Sum(r => r.TotalCompensation);
                var departmentCount = employeeDepartmentCountsList
                    .FirstOrDefault(edc => edc.EmployeeId == employee.Id)?.DepartmentCount ?? 1;

                var compensationPerDepartment = totalCompensation / departmentCount;

                var departments = await _context.DepartmentEmployee
                    .Where(de => de.EmployeeId == employee.Id)
                    .Select(de => de.Department)
                    .ToListAsync();

                foreach (var department in departments)
                {
                    var departmentName = department.Name;

                    if (!departmentCompensations.ContainsKey(departmentName))
                    {
                        departmentCompensations[departmentName] = 0;
                        departmentSharedCompensations[departmentName] = 0;
                    }

                    departmentCompensations[departmentName] = Math.Round(departmentCompensations[departmentName] + totalCompensation, 2);
                    departmentSharedCompensations[departmentName] = Math.Round(departmentSharedCompensations[departmentName] + compensationPerDepartment, 2);
                }
            }

            var result = departmentCompensations
                .Where(dc => dc.Value > 0)
                .Select(dc => new DepartmentCompensationViewModel
                {
                    DepartmentName = dc.Key,
                    TotalCompensation = Math.Round(dc.Value, 2),
                    Users = employees
                        .Where(e => _context.DepartmentEmployee.Any(de => de.EmployeeId == e.Id && de.Department.Name == dc.Key))
                        .Select(e => new CompensationEmployeeViewModel
                        {
                            EmployeeFullName = e.FullName,
                            TotalCompensation = Math.Round(e.Remunerations.Sum(r => r.TotalCompensation), 2),
                            SharedCompensationByDepartment = Math.Round(departmentSharedCompensations[dc.Key], 2)
                        }).ToList(),
                    SharedCompensation = Math.Round(departmentSharedCompensations[dc.Key], 2)
                }).ToList();

            return Ok(result);
        }
    }
}
