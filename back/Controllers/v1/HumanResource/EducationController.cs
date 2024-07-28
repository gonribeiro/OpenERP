using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.HumanResource;
using OpenERP.ViewModels.Global;
using OpenERP.ViewModels.HumanResource.Educations;

namespace OpenERP.Controllers.v1.HumanResource
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EducationController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/employees/{employeeId:int}/educations")]
        public async Task<List<IndexEducationViewModel>> GetByEmployeeIdAsync([FromRoute] int employeeId)
        {
            var educations = await _context.EducationHistories
                .Where(c => c.EmployeeId == employeeId)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexEducationViewModel
                {
                    Id = c.Id,
                    Institution = c.Institution.FullName,
                    Course = c.Course,
                    EducationLevel = c.EducationLevel,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd"),
                })
                .ToListAsync();

            return educations;
        }

        [HttpPost("v1/educations")]
        public async Task<IActionResult> PostAsync([FromBody] CreateEducationViewModel model)
        {
            try
            {
                var education = new EducationHistory
                {
                    EmployeeId = model.EmployeeId,
                    InstitutionId = model.InstitutionId,
                    Course = model.Course,
                    EducationLevel = model.EducationLevel,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                };
                await _context.EducationHistories.AddAsync(education);
                await _context.SaveChangesAsync();

                return Ok(new {
                    education,
                    message = "Created successfully",
                    redirectTo = $"employees/{education.EmployeeId}/educations/{education.Id}/edit"
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

        [HttpGet("v1/educations/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var education = await _context
                   .EducationHistories
                   .Select(c => new GetEducationViewModel
                   {
                       Id = c.Id,
                       InstitutionId = c.InstitutionId,
                       Course = c.Course,
                       EducationLevel = c.EducationLevel,
                       StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                       EndDate = c.EndDate.ToString("yyyy-MM-dd"),
                   })
                   .FirstOrDefaultAsync(x => x.Id == id);

                if (education == null)
                    return NotFound("Education not found");

                return Ok(education);
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

        [HttpPut("v1/educations/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UpdateEducationViewModel model)
        {
            try
            {
                var education = await _context.EducationHistories.FirstOrDefaultAsync(x => x.Id == id);
                if (education == null)
                    return NotFound("Education not found");

                education.InstitutionId = model.InstitutionId;
                education.Course = model.Course;
                education.EducationLevel = model.EducationLevel;
                education.StartDate = model.StartDate;
                education.EndDate = model.EndDate;

                _context.EducationHistories.Update(education);
                await _context.SaveChangesAsync();

                return Ok(new { education, message = "Saved successfully" });
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

        [HttpDelete("v1/educations/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var education = await _context.EducationHistories.FirstOrDefaultAsync(x => x.Id == id);
                if (education == null)
                    return NotFound("Education not found");

                _context.EducationHistories.Remove(education);
                await _context.SaveChangesAsync();

                return Ok(new {
                    education,
                    message = "Deleted successfully",
                    redirectTo = $"employees/{education.EmployeeId}/educations"
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
    }
}
