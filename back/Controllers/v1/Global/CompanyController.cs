using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Enums.Global;
using OpenERP.Models.Global;
using OpenERP.Services.Global;
using OpenERP.ViewModels.Global.Companies;
using OpenERP.ViewModels.Global.Contacts;
using System.Data;

namespace OpenERP.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CompanyController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly AppDbContext _context;

        public CompanyController(
            ContactService contactService,
            [FromServices] AppDbContext context)
        {
            _contactService = contactService;
            _context = context;
        }

        [HttpGet("v1/companies")]
        public async Task<List<GetCompanyViewModel>> GetCompanies()
        {
            var companies = await _context.Companies
                .OrderByDescending(c => c.Id)
                .Select(c => new GetCompanyViewModel
                {
                    Id = c.Id,
                    LegalName = c.LegalName,
                    TradeName = c.TradeName,
                    FullName = c.FullName,
                    Type = c.Type.ToString(),
                    Address = c.Address,
                    City = c.City.Name,
                })
                .ToListAsync();

            return companies;
        }

        [HttpGet("v1/companies/{type}")]
        public async Task<IActionResult> GetCompaniesByTypeAsync([FromRoute] string type)
        {
            if (!Enum.TryParse(type, out CompanyType companyType))
                return BadRequest("Invalid company type");

            var companies = await _context.Companies
                .Where(c => c.Type == companyType)
                .OrderByDescending(c => c.Id)
                .Select(c => new GetCompanyViewModel
                {
                    Id = c.Id,
                    Name = type == "Company" ? c.TradeName : c.FullName,
                })
                .ToListAsync();

            return Ok(companies);
        }

        [HttpPost("v1/companies")]
        public async Task<IActionResult> PostAsync([FromBody] CompanyViewModel model)
        {
            var nameExists = await _context.Companies.FirstOrDefaultAsync(c => c.LegalName == model.LegalName);
            if (nameExists != null)
                return BadRequest("Company already exists");

            if (!Enum.TryParse(model.Type, true, out CompanyType companyType))
                return BadRequest("Invalid company type");

            var company = new Company
            {
                LegalName = model.LegalName,
                TradeName = model.TradeName,
                Type = companyType,
                CityId = model.CityId,
                Address = model.Address,
                ZipCode = model.ZipCode,
                ProductAndServiceDescription = model.ProductAndServiceDescription,
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Companies.AddAsync(company);
                    await _context.SaveChangesAsync();

                    if (model.Contacts != null)
                        await _contactService.UpsertContacts("Company", company.Id, model.Contacts);

                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ex.Message);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Internal Server Error");
                }
            }

            return Ok(new
            {
                company,
                message = "Created successfully",
                redirectTo = $"companies/{company.Id}/edit"
            });
        }

        [HttpGet("v1/companies/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var company = await _context
                    .Companies
                    .Select(c => new CompanyViewModel
                    {
                        Id = c.Id,
                        LegalName = c.LegalName,
                        TradeName = c.TradeName,
                        Type = c.Type.ToString(),
                        Address = c.Address,
                        ZipCode = c.ZipCode,
                        CityId = c.CityId,
                        ProductAndServiceDescription = c.ProductAndServiceDescription,
                        Contacts = c.Contacts
                           .Where(c => c.ModelType == "Company")
                           .Select(co => new ContactViewModel
                           {
                               Id = co.Id,
                               Type = co.Type.ToString(),
                               Information = co.Information,
                               ContactName = co.ContactName,
                           }).ToList(),

                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (company == null)
                    return NotFound("Company not found");

                return Ok(company);
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

        [HttpPut("v1/companies/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] CompanyViewModel model)
        {
            var nameExists = await _context.Companies.FirstOrDefaultAsync(c => c.LegalName == model.LegalName);
            if (nameExists != null && nameExists.Id != id)
                return BadRequest("Company name already exists");

            if (!Enum.TryParse(model.Type, true, out CompanyType companyType))
                return BadRequest("Invalid company type");

            var company = await _context
                .Companies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
                return NotFound("Company not found");

            company.LegalName = model.LegalName;
            company.TradeName = model.TradeName;
            company.Type = companyType;
            company.CityId = model.CityId;
            company.Address = model.Address;
            company.ZipCode = model.ZipCode;
            company.ProductAndServiceDescription = model.ProductAndServiceDescription;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();

                    await _contactService.RemoveContacts("Company", company.Id, model.Contacts);
                    if (model.Contacts != null)
                        await _contactService.UpsertContacts("Company", company.Id, model.Contacts);

                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, ex.Message);
                }
                catch
                {
                    return StatusCode(500, "Internal Server Error");
                }
            }

            return Ok(new { company, message = "Saved successfully" });
        }

        [HttpDelete("v1/companies/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var company = await _context
                    .Companies
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (company == null)
                    return NotFound("Company not found");

                _context.Contacts.RemoveRange(company.Contacts);
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    company,
                    message = "Deleted successfully",
                    redirectTo = $"companies/"
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
