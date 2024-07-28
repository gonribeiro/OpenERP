using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.HumanResource;
using OpenERP.ViewModels.Global;
using OpenERP.ViewModels.HumanResource.Vacations;

namespace OpenERP.Controllers.v1.HumanResource
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class VacationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VacationController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/employees/{employeeId:int}/vacations")]
        public async Task<List<IndexVacationViewModel>> GetByEmployeeIdAsync([FromRoute] int employeeId)
        {
            var vacations = await _context.Vacations
                .Where(c => c.EmployeeId == employeeId)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexVacationViewModel
                {
                    Id = c.Id,
                    Type = c.Type.ToString(),
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd"),
                    Reason = c.Reason,
                    ApprovedByName = c.ApprovedBy.FullName,
                })
                .ToListAsync();

            return vacations;
        }

        [HttpPost("v1/vacations")]
        public async Task<IActionResult> PostAsync([FromBody] CreateVacationViewModel model)
        {
            try
            {
                if (!Enum.TryParse(model.Type, true, out VacationType vacationType))
                    return BadRequest("Invalid vacation type");

                var vacation = new Vacation
                {
                    EmployeeId = model.EmployeeId,
                    Type = vacationType,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Reason = model.Reason,
                    ApprovedById = model.ApprovedById,
            };
                await _context.Vacations.AddAsync(vacation);
                await _context.SaveChangesAsync();

                return Ok(new {
                    vacation,
                    message = "Created successfully",
                    redirectTo = $"employees/{vacation.EmployeeId}/vacations/{vacation.Id}/edit"
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

        [HttpGet("v1/vacations/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var vacation = await _context
                   .Vacations
                   .Select(c => new GetVacationViewModel
                   {
                       Id = c.Id,
                       Type = c.Type.ToString(),
                       StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                       EndDate = c.EndDate.ToString("yyyy-MM-dd"),
                       Reason = c.Reason,
                       ApprovedById = c.ApprovedById,
                   })
                   .FirstOrDefaultAsync(x => x.Id == id);

                if (vacation == null)
                    return NotFound("Vacation not found");

                return Ok(vacation);
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

        [HttpPut("v1/vacations/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UpdateVacationViewModel model)
        {
            try
            {
                var vacation = await _context.Vacations.FirstOrDefaultAsync(x => x.Id == id);
                if (vacation == null)
                    return NotFound("Vacation not found");

                if (!Enum.TryParse(model.Type, true, out VacationType vacationType))
                    return BadRequest("Invalid vacation type");

                vacation.Type = vacationType;
                vacation.StartDate = model.StartDate;
                vacation.EndDate = model.EndDate;
                vacation.Reason = model.Reason;
                vacation.ApprovedById = model.ApprovedById;

                _context.Vacations.Update(vacation);
                await _context.SaveChangesAsync();

                return Ok(new { vacation, message = "Saved successfully" });
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

        [HttpDelete("v1/vacations/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var vacation = await _context.Vacations.FirstOrDefaultAsync(x => x.Id == id);
                if (vacation == null)
                    return NotFound("Vacation not found");

                _context.Vacations.Remove(vacation);
                await _context.SaveChangesAsync();

                return Ok(new {
                    vacation,
                    message = "Deleted successfully",
                    redirectTo = $"employees/{vacation.EmployeeId}/vacations"
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

        [HttpGet("v1/vacations/vacationsByDepartment")]
        public async Task<IActionResult> GetVacationsByDepartmentAsync()
        {
            var today = DateTime.Today;

            var vacations = await _context.Vacations
                .Include(v => v.Employee)
                    .ThenInclude(e => e.DepartmentEmployee)
                    .ThenInclude(de => de.Department)
                .Include(v => v.ApprovedBy)
                .Where(v => v.EndDate > today && v.Employee.InactiveDate == null)
                .Select(v => new
                {
                    v.Id,
                    v.Type,
                    v.StartDate,
                    v.EndDate,
                    v.Reason,
                    ApprovedByName = v.ApprovedBy != null ? v.ApprovedBy.FullName : null,
                    EmployeeName = v.Employee.FirstName + " " + v.Employee.LastName,
                    Departments = v.Employee.DepartmentEmployee.Select(de => de.Department.Name).ToList()
                })
                .ToListAsync();

            var vacationsByDepartment = vacations
                .SelectMany(v => v.Departments.Select(d => new
                {
                    DepartmentName = d,
                    Vacation = new IndexVacationViewModel
                    {
                        Id = v.Id,
                        Type = v.Type.ToString(),
                        StartDate = v.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = v.EndDate.ToString("yyyy-MM-dd"),
                        Reason = v.Reason,
                        ApprovedByName = v.ApprovedByName,
                        EmployeeName = v.EmployeeName
                    }
                }))
                .GroupBy(v => v.DepartmentName)
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    Vacations = g.Select(v => v.Vacation).ToList()
                })
                .ToList();

            return Ok(vacationsByDepartment);
        }
    }
}
