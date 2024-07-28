using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Global;
using OpenERP.ViewModels.Global.Cities;

namespace OpenERP.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CityController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/cities")]
        public async Task<List<IndexCityViewModel>> GetCitiesAsync()
        {
            var cities = await _context.Cities
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexCityViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    StateName = c.State.Name,
                })
                .ToListAsync();

            return cities;
        }

        [HttpPost("v1/cities")]
        public async Task<IActionResult> PostAsync([FromBody] CityViewModel model)
        {
            try
            {
                var nameExists = await _context.Cities.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null)
                    return BadRequest("City already exists");

                var city = new City
                {
                    Name = model.Name,
                    StateId = model.StateId,
                };
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();

                return Ok(new {
                    city,
                    message = "Created successfully",
                    redirectTo = $"cities/{city.Id}/edit"
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

        [HttpGet("v1/cities/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
                if (city == null)
                    return NotFound("City not found");

                return Ok(city);
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

        [HttpPut("v1/cities/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] CityViewModel model)
        {
            try
            {
                var nameExists = await _context.Cities.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null && nameExists.Id != id)
                    return BadRequest("City already exists");

                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
                if (city == null)
                    return NotFound("City not found");

                city.Name = model.Name;
                city.StateId = model.StateId;

                _context.Cities.Update(city);
                await _context.SaveChangesAsync();

                return Ok(new { city, message = "Saved successfully" });
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

        [HttpDelete("v1/cities/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
                if (city == null)
                    return NotFound("City not found");

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    city,
                    message = "Deleted successfully",
                    redirectTo = $"cities/"
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
