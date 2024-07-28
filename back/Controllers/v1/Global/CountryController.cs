using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Global;
using OpenERP.ViewModels.Global.Countries;

namespace OpenERP.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CountryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CountryController(
            [FromServices] AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/countries")]
        public async Task<List<Country>> GetCountriesAsync()
        {
            var countries = await _context.Countries.OrderByDescending(c => c.Id).ToListAsync();

            return countries;
        }

        [HttpGet("v1/countries/nationalities")]
        public async Task<IActionResult> GetNationalitiesAsync()
        {
            var nationalities = await _context.Countries
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexNationalityViewModel
                {
                    Id = c.Id,
                    Name = c.Nationality,
                })
                .ToListAsync();

            return Ok(nationalities);
        }

        [HttpPost("v1/countries")]
        public async Task<IActionResult> PostAsync([FromBody] CountryViewModel model)
        {
            try
            {
                var nameExists = await _context.Countries.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null)
                    return BadRequest("Country name already exists");

                var country = new Country
                {
                    Name = model.Name,
                    Nationality = model.Nationality,
                };
                await _context.Countries.AddAsync(country);
                await _context.SaveChangesAsync();

                return Ok(new {
                    country,
                    message = "Created successfully",
                    redirectTo = $"countries/{country.Id}/edit"
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

        [HttpGet("v1/countries/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var country = await _context
                    .Countries
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (country == null)
                    return NotFound("Country not found");

                return Ok(country);
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

        [HttpPut("v1/countries/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] CountryViewModel model)
        {
            try
            {
                var nameExists = await _context.Countries.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (nameExists != null && nameExists.Id != id)
                    return BadRequest("Country name already exists");

                var country = await _context
                    .Countries
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (country == null)
                    return NotFound("Country not found");

                country.Name = model.Name;
                country.Nationality = model.Nationality;

                _context.Countries.Update(country);
                await _context.SaveChangesAsync();

                return Ok(new { country, message = "Saved successfully" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch
            {
                return StatusCode(500,"Internal Server Error");
            }
        }

        [HttpDelete("v1/countries/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var country = await _context
                    .Countries
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (country == null)
                    return NotFound("Country not found");

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    country,
                    message = "Deleted successfully",
                    redirectTo = $"countries"
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
