using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Enums.Global;
using OpenERP.Models.HumanResource;
using OpenERP.ViewModels.HumanResource.Remunerations;

namespace OpenERP.Controllers.v1.HumanResource
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RemunerationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RemunerationController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/Employees/{employeeId:int}/remunerations")]
        public async Task<List<IndexRemunerationViewModel>> GetByEmployeeIdAsync([FromRoute] int employeeId)
        {
            var remunerations = await _context.Remunerations
                .Where(c => c.EmployeeId == employeeId)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexRemunerationViewModel
                {
                    Id = c.Id,
                    Currency = c.Currency.ToString(),
                    BaseSalary = c.BaseSalary,
                    Bonus = c.Bonus,
                    Commission = c.Commission,
                    OtherAllowances = c.OtherAllowances,
                    OtherAllowancesDescription = c.OtherAllowancesDescription,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = c.EndDate.HasValue ? c.EndDate.Value.ToString("yyyy-MM-dd") : null,
                })
                .ToListAsync();

            return remunerations;
        }

        [HttpPost("v1/remunerations")]
        public async Task<IActionResult> PostAsync([FromBody] CreateRemunerationViewModel model)
        {
            try
            {
                if (!Enum.TryParse(model.Currency, true, out Currency currency))
                    return BadRequest("Invalid currency type");

                var remuneration = new Remuneration
                {
                    EmployeeId = model.EmployeeId,
                    Currency = currency,
                    BaseSalary = model.BaseSalary,
                    Bonus = model.Bonus,
                    Commission = model.Commission,
                    OtherAllowances = model.OtherAllowances,
                    OtherAllowancesDescription = model.OtherAllowancesDescription,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                };
                await _context.Remunerations.AddAsync(remuneration);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    remuneration,
                    message = "Created successfully",
                    redirectTo = $"employees/{remuneration.EmployeeId}/remunerations/{remuneration.Id}/edit"
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

        [HttpGet("v1/remunerations/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var remuneration = await _context
                    .Remunerations
                    .Select(c => new GetRemunerationViewModel
                    {
                        Id = c.Id,
                        Currency = c.Currency.ToString(),
                        BaseSalary = c.BaseSalary,
                        Bonus = c.Bonus,
                        Commission = c.Commission,
                        OtherAllowances = c.OtherAllowances,
                        OtherAllowancesDescription = c.OtherAllowancesDescription,
                        StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = c.EndDate.HasValue ? c.EndDate.Value.ToString("yyyy-MM-dd") : null,
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (remuneration == null)
                    return NotFound("Remuneration not found");

                return Ok(remuneration);
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

        [HttpPut("v1/remunerations/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UpdateRemunerationViewModel model)
        {
            try
            {
                var remuneration = await _context.Remunerations.FirstOrDefaultAsync(x => x.Id == id);
                if (remuneration == null)
                    return NotFound("Remuneration not found");

                if (!Enum.TryParse(model.Currency, true, out Currency currency))
                    return BadRequest("Invalid currency type");

                remuneration.Currency = currency;
                remuneration.BaseSalary = model.BaseSalary;
                remuneration.Bonus = model.Bonus;
                remuneration.Commission = model.Commission;
                remuneration.OtherAllowances = model.OtherAllowances;
                remuneration.OtherAllowancesDescription = model.OtherAllowancesDescription;
                remuneration.StartDate = model.StartDate;
                remuneration.EndDate = model.EndDate;

                _context.Remunerations.Update(remuneration);
                await _context.SaveChangesAsync();

                return Ok(new { remuneration, message = "Saved successfully" });
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

        [HttpDelete("v1/remunerations/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var remuneration = await _context.Remunerations.FirstOrDefaultAsync(x => x.Id == id);
                if (remuneration == null)
                    return NotFound("Remuneration not found");

                _context.Remunerations.Remove(remuneration);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    remuneration,
                    message = "Deleted successfully",
                    redirectTo = $"employees/{remuneration.EmployeeId}/remunerations"
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
