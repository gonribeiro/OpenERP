using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Global;
using OpenERP.ViewModels.Global.States;

namespace OpenERP.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StateController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StateController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/states")]
        public async Task<List<IndexStateViewModel>> GetStatesAsync()
        {
            var states = await _context.States
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexStateViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    CountryName = c.Country.Name,
                })
                .ToListAsync();

            return states;
        }

        [HttpPost("v1/states")]
        public async Task<IActionResult> PostAsync([FromBody] StateViewModel model)
        {
            try
            {
                var nameExists = await _context.States.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null)
                    return BadRequest("State already exists");

                var state = new State
                {
                    Name = model.Name,
                    CountryId = model.CountryId,
                };
                await _context.States.AddAsync(state);
                await _context.SaveChangesAsync();

                return Ok(new {
                    state,
                    message = "Created successfully",
                    redirectTo = $"states/{state.Id}/edit"
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

        [HttpGet("v1/states/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var state = await _context
                    .States
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (state == null)
                    return NotFound("State not found");

                return Ok(state);
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

        [HttpPut("v1/states/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] StateViewModel model)
        {
            try
            {
                var nameExists = await _context.States.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null && nameExists.Id != id)
                    return BadRequest("State already exists");

                var state = await _context
                    .States
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (state == null)
                    return NotFound("State not found");

                state.Name = model.Name;
                state.CountryId = model.CountryId;

                _context.States.Update(state);
                await _context.SaveChangesAsync();

                return Ok(new { state, message = "Saved successfully" });
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

        [HttpDelete("v1/states/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var state = await _context
                    .States
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (state == null)
                    return NotFound("State not found");

                _context.States.Remove(state);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    state,
                    message = "Deleted successfully",
                    redirectTo = $"states/"
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
