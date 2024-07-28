using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Enums.Global;
using OpenERP.Models.HumanResource;
using OpenERP.ViewModels.Global;
using OpenERP.ViewModels.HumanResource.Jobs;

namespace OpenERP.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/jobs")]
        public async Task<List<IndexJobViewModel>> GetJobsAsync()
        {
            var jobs = await _context.Jobs
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexJobViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return jobs;
        }

        [HttpPost("v1/jobs")]
        public async Task<IActionResult> PostAsync(
            [FromBody] JobViewModel model)
        {
            try
            {
                var nameExists = await _context.Jobs.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null)
                    return BadRequest("Job name already exists");

                if (!Enum.TryParse(model.Currency, true, out Currency currency))
                    return BadRequest("Invalid currency");

                var job = new Job
                {
                    Name = model.Name,
                    Currency = currency,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                };
                await _context.Jobs.AddAsync(job);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    job,
                    message = "Created successfully",
                    redirectTo = $"jobs/{job.Id}/edit"
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

        [HttpGet("v1/jobs/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id)
        {
            try
            {
                var job = await _context
                    .Jobs
                    .Select(c => new GetJobViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Currency = c.Currency.ToString(),
                        MinSalary = c.MinSalary,
                        MaxSalary = c.MaxSalary,

                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (job == null)
                    return NotFound("Job not found");

                return Ok(job);
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

        [HttpPut("v1/jobs/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] JobViewModel model)
        {
            try
            {
                var nameExists = await _context.Jobs.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null && nameExists.Id != id)
                    return BadRequest("Job name already exists");

                if (!Enum.TryParse(model.Currency, true, out Currency currency))
                    return BadRequest("Invalid currency");

                var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
                if (job == null)
                    return NotFound("Job not found");

                job.Name = model.Name;
                job.Currency = currency;
                job.MinSalary = model.MinSalary;
                job.MaxSalary = model.MaxSalary;

                _context.Jobs.Update(job);
                await _context.SaveChangesAsync();

                return Ok(new { job, message = "Saved successfully" });
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

        [HttpDelete("v1/jobs/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id)
        {
            try
            {
                var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
                if (job == null)
                    return NotFound("Job not found");

                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    job,
                    message = "Deleted successfully",
                    redirectTo = $"jobs"
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
