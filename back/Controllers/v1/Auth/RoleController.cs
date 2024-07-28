using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Auth;
using OpenERP.ViewModels.Auth.Users;

namespace OpenERP.Controllers.v1.HumanResource
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/roles")]
        public async Task<List<GetRoleViewModel>> GetRolesAsync()
        {
            var roles = await _context.Roles
                .OrderByDescending(c => c.Id)
                .Select(c => new GetRoleViewModel
                {
                    Id = c.Id,
                    InactiveDate = c.InactiveDate.HasValue ? c.InactiveDate.Value.ToString("yyyy-MM-dd") : null,
                    Name = c.Name,
                })
                .ToListAsync();

            return roles;
        }

        [HttpGet("v1/roles/actives")]
        public async Task<List<GetRoleViewModel>> GetActiveRolesAsync()
        {
            var roles = await _context.Roles
                .Where(c => c.InactiveDate == null)
                .OrderByDescending(c => c.Id)
                .Select(c => new GetRoleViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return roles;
        }

        [HttpPost("v1/roles")]
        public async Task<IActionResult> PostAsync([FromBody] RoleViewModel model)
        {
            try
            {
                var name = await _context.Roles.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (name != null)
                    return BadRequest("Role already exists");

                var role = new Role
                {
                    Name = model.Name,
                };

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    role,
                    message = "Created successfully",
                    redirectTo = $"roles/{role.Id}/edit"
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

        [HttpGet("v1/roles/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var role = await _context
                    .Roles
                    .Select(c => new GetRoleViewModel
                    {
                        Id = c.Id,
                        InactiveDate = c.InactiveDate.HasValue ? c.InactiveDate.Value.ToString("yyyy-MM-dd") : null,
                        Name = c.Name,
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (role == null)
                    return NotFound("Role not found");

                return Ok(role);
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

        [HttpPut("v1/roles/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] RoleViewModel model)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
                if (role == null)
                    return NotFound("Role not found");

                var name = await _context.Roles.FirstOrDefaultAsync(c => c.Id != id && c.Name == model.Name);
                if (name != null)
                    return BadRequest("Role already exists");

                role.InactiveDate = model.InactiveDate;
                role.Name = model.Name;

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return Ok(new { role, message = "Saved successfully" });
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
