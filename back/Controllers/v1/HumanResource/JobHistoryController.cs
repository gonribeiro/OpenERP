using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.HumanResource;
using OpenERP.ViewModels.Global;
using OpenERP.ViewModels.HumanResource.JobHistories;

namespace OpenERP.Controllers.v1.HumanResource
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class JobHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobHistoryController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/Employees/{employeeId:int}/jobHistories")]
        public async Task<List<IndexJobHistoryViewModel>> GetByEmployeeIdAsync([FromRoute] int employeeId)
        {
            var jobHistories = await _context.JobHistories
                .Where(c => c.EmployeeId == employeeId)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexJobHistoryViewModel
                {
                    Id = c.Id,
                    Job = c.Job.Name,
                    Department = c.Department.Name,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = c.EndDate.HasValue ? c.EndDate.Value.ToString("yyyy-MM-dd") : null,
                })
                .ToListAsync();

            return jobHistories;
        }

        [HttpPost("v1/jobHistories")]
        public async Task<IActionResult> PostAsync([FromBody] CreateJobHistoryViewModel model)
        {
            try
            {
                var jobHistory = new JobHistory
                {
                    EmployeeId = model.EmployeeId,
                    JobId = model.JobId,
                    DepartmentId = model.DepartmentId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                };
                await _context.JobHistories.AddAsync(jobHistory);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    jobHistory,
                    message = "Created successfully",
                    redirectTo = $"employees/{jobHistory.EmployeeId}/jobHistories/{jobHistory.Id}/edit"
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

        [HttpGet("v1/jobHistories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var jobHistory = await _context
                    .JobHistories
                    .Select(c => new GetJobHistoryViewModel
                    {
                        Id = c.Id,
                        EmployeeId = c.EmployeeId,
                        JobId = c.EmployeeId,
                        DepartmentId = c.DepartmentId,
                        StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = c.EndDate.HasValue ? c.EndDate.Value.ToString("yyyy-MM-dd") : null,
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (jobHistory == null)
                    return NotFound("Job history not found");

                return Ok(jobHistory);
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

        [HttpPut("v1/jobHistories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UpdateJobHistoryViewModel model)
        {
            try
            {
                var jobHistory = await _context
                    .JobHistories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (jobHistory == null)
                    return NotFound("Job history not found");

                jobHistory.JobId = model.JobId;
                jobHistory.DepartmentId = model.DepartmentId;
                jobHistory.StartDate = model.StartDate;
                jobHistory.EndDate = model.EndDate;

                _context.JobHistories.Update(jobHistory);
                await _context.SaveChangesAsync();

                return Ok(new { jobHistory, message = "Saved successfully" });
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

        [HttpDelete("v1/jobHistories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            try
            {
                var jobHistory = await context
                    .JobHistories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (jobHistory == null)
                    return NotFound("Job history not found");

                context.JobHistories.Remove(jobHistory);
                await context.SaveChangesAsync();

                return Ok(new
                {
                    jobHistory,
                    message = "Deleted successfully",
                    redirectTo = $"employees/{jobHistory.EmployeeId}/jobHistories"
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
