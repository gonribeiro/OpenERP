using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.HumanResource;
using OpenERP.Services.Global;
using OpenERP.ViewModels.Global.Contacts;
using OpenERP.ViewModels.HumanResource.Departments;
using OpenERP.ViewModels.HumanResource.Employees;

namespace OpenERP.Controllers.v1
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly AppDbContext _context;
        private readonly FileStorageService _fileStorageService;
        private readonly RefreshTokenService _refreshTokenService;

        public EmployeeController(
            ContactService contactService,
            [FromServices] AppDbContext context,
            RefreshTokenService refreshTokenService,
            FileStorageService fileStorageService)
        {
            _contactService = contactService;
            _context = context;
            _fileStorageService = fileStorageService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpGet("v1/employees")]
        public async Task<List<IndexEmployeeViewModel>> GetActiveEmployeesAsync()
        {
            var employees = await _context.Employees
                .Where(c => c.InactiveDate == null)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexEmployeeViewModel
                {
                    Id = c.Id,
                    Name = c.FullName,
                    City = c.City.Name,
                    Nationality = c.Nationality.Name,
                    Departments = string.Join(", ", c.DepartmentEmployee.Select(ud => ud.Department.Name)),
                })
                .ToListAsync();

            return employees;
        }

        [HttpGet("v1/employees/inactives")]
        public async Task<List<IndexEmployeeViewModel>> GetInactiveEmployeesAsync()
        {
            var employees = await _context.Employees
                .Where(c => c.InactiveDate != null)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexEmployeeViewModel
                {
                    Id = c.Id,
                    InactiveDate = c.InactiveDate.Value.ToString("yyyy-MM-dd"),
                    Name = c.FullName,
                    City = c.City.Name,
                    Nationality = c.Nationality.Name,
                    Departments = string.Join(", ", c.DepartmentEmployee.Select(ud => ud.Department.Name)),
                })
                .ToListAsync();

            return employees;
        }

        [HttpGet("v1/employees/withoutUserAccount")]
        public async Task<List<IndexEmployeeViewModel>> GetEmployeesWithoutUserAccountAsync()
        {
            var employees = await _context.Employees
                .Where(c => c.InactiveDate == null && c.User == null)
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexEmployeeViewModel
                {
                    Id = c.Id,
                    Name = c.FullName,
                })
                .ToListAsync();

            return employees;
        }

        [HttpPost("v1/employees")]
        public async Task<IActionResult> PostAsync([FromBody] EmployeeViewModel model)
        {
            var socialSecurityNumber = await _context.Employees
                .FirstOrDefaultAsync(c => c.SocialSecurityNumber == model.SocialSecurityNumber);
            if (socialSecurityNumber != null)
                return BadRequest("Social Security Number already exists");

            var passportNumber = await _context.Employees
                .FirstOrDefaultAsync(c => c.PassportNumber == model.PassportNumber && c.PassportNumber != null);
            if (passportNumber != null)
                return BadRequest("Passport Number already exists");

            var driverLicenseNumber = await _context.Employees
                .FirstOrDefaultAsync(c => c.DriverLicenseNumber == model.DriverLicenseNumber && c.DriverLicenseNumber != null);
            if ( driverLicenseNumber != null)
                return BadRequest("Driver License Number already exists");

            if (!Enum.TryParse(model.MaritalStatus, true, out MaritalStatus maritalStatus))
                return BadRequest("Invalid marital status type");

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.Birthdate,
                MaritalStatus = maritalStatus,
                NationalityId = model.NationalityId,
                PlaceOfBirth = model.PlaceOfBirth,
                Address = model.Address,
                ZipCode = model.ZipCode,
                CityId = model.CityId,
                SocialSecurityNumber = model.SocialSecurityNumber,
                PassportNumber = model.PassportNumber,
                DriverLicenseNumber = model.DriverLicenseNumber,
                BankId = model.BankId,
                AccountNumber = model.AccountNumber,
                RoutingNumber = model.RoutingNumber,
            };

            if (model.DepartmentIds != null)
            {
                employee.DepartmentEmployee = new List<DepartmentEmployee>();

                foreach (var departmentId in model.DepartmentIds)
                {
                    var departmentEmployee = new DepartmentEmployee
                    {
                        EmployeeId = employee.Id,
                        DepartmentId = departmentId
                    };

                    employee.DepartmentEmployee.Add(departmentEmployee);
                }
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();

                    if (model.Contacts != null)
                        await _contactService.UpsertContacts("Employee", employee.Id, model.Contacts);
                    if (model.Relatives != null)
                        await _contactService.UpsertContacts("Employee", employee.Id, model.Relatives);

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
                employee,
                message = "Created successfully",
                redirectTo = $"employees/{employee.Id}/edit"
            });
        }

        [HttpGet("v1/employees/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.Contacts)
                    .Include(e => e.DepartmentEmployee)
                        .ThenInclude(e => e.Department)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null) return NotFound("Employee not found");

                var photoId = await _context.FileStorages
                    .Where(fs => fs.ModelType == "Employee" && fs.ModelId == id)
                    .Select(fs => fs.Id)
                    .FirstOrDefaultAsync();

                string? photo = null;
                if (photoId != 0)
                {
                    var (memoryStream, fileName) = await _fileStorageService.DownloadFileAsync(photoId);
                    byte[] photoBytes = memoryStream.ToArray();
                    string photoBase64 = Convert.ToBase64String(photoBytes);
                    photo = $"data:image/jpeg;base64,{photoBase64}";
                }

                var employeeViewModel = new GetEmployeeViewModel
                {
                    Id = employee.Id,
                    InactiveDate = employee.InactiveDate.HasValue ? employee.InactiveDate.Value.ToString("yyyy-MM-dd") : null,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Birthdate = employee.Birthdate.ToString("yyyy-MM-dd"),
                    MaritalStatus = employee.MaritalStatus.ToString(),
                    NationalityId = employee.NationalityId,
                    PlaceOfBirth = employee.PlaceOfBirth,
                    Contacts = employee.Contacts
                        .Where(c => c.ModelType == "Employee" && c.ContactName == null)
                        .Select(co => new ContactViewModel
                        {
                            Id = co.Id,
                            Type = co.Type.ToString(),
                            Information = co.Information,
                        }).ToList(),
                    DepartmentIds = employee.DepartmentEmployee.Select(ud => ud.Department.Id).ToList(),
                    Address = employee.Address,
                    ZipCode = employee.ZipCode,
                    CityId = employee.CityId,
                    SocialSecurityNumber = employee.SocialSecurityNumber,
                    PassportNumber = employee.PassportNumber,
                    DriverLicenseNumber = employee.DriverLicenseNumber,
                    BankId = employee.BankId,
                    AccountNumber = employee.AccountNumber,
                    RoutingNumber = employee.RoutingNumber,
                    Relatives = employee.Contacts
                        .Where(c => c.ModelType == "Employee" && c.ContactName != null)
                        .Select(co => new ContactViewModel
                        {
                            Id = co.Id,
                            Type = co.Type.ToString(),
                            Information = co.Information,
                            ContactName = co.ContactName,
                            ContactRelationType = co.ContactRelationType.ToString(),
                        }).ToList(),
                    PhotoId = photoId != 0 ? photoId : (int?)null,
                    Photo = photo
                };

                return Ok(employeeViewModel);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("v1/employees/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EmployeeViewModel model)
        {
            var employee = await _context.Employees
                .Include(u => u.DepartmentEmployee)
                .Include(u => u.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
                return NotFound("Page not found");

            var socialSecurityNumber = await _context.Employees
                .FirstOrDefaultAsync(c => c.SocialSecurityNumber == model.SocialSecurityNumber);
            if (socialSecurityNumber != null && socialSecurityNumber.Id != id)
                return BadRequest("Social Security Number already exists");

            var passportNumber = await _context.Employees
                .FirstOrDefaultAsync(c => c.PassportNumber == model.PassportNumber && c.PassportNumber != null);
            if (passportNumber != null && passportNumber.Id != id)
                return BadRequest("Passport Number already exists");

            var driverLicenseNumber = await _context.Employees
                .FirstOrDefaultAsync(c => c.DriverLicenseNumber == model.DriverLicenseNumber && c.DriverLicenseNumber != null);
            if (driverLicenseNumber != null && driverLicenseNumber.Id != id)
                return BadRequest("Driver License Number already exists");

            if (!Enum.TryParse(model.MaritalStatus, true, out MaritalStatus maritalStatus))
                return BadRequest("Invalid marital status type");

            employee.InactiveDate = model.InactiveDate;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Birthdate = model.Birthdate;
            employee.MaritalStatus = maritalStatus;
            employee.NationalityId = model.NationalityId;
            employee.PlaceOfBirth = model.PlaceOfBirth;
            employee.Address = model.Address;
            employee.ZipCode = model.ZipCode;
            employee.CityId = model.CityId;
            employee.SocialSecurityNumber = model.SocialSecurityNumber;
            employee.PassportNumber = model.PassportNumber;
            employee.DriverLicenseNumber = model.DriverLicenseNumber;
            employee.BankId = model.BankId;
            employee.AccountNumber = model.AccountNumber;
            employee.RoutingNumber = model.RoutingNumber;

            if (model.InactiveDate != null && employee.User != null)
            {
                employee.User.InactiveDate = model.InactiveDate;
                await _refreshTokenService.RevokeRefreshTokensAsync(employee.User.Id);
            }

            foreach (var departmentEmployee in employee.DepartmentEmployee.ToList())
            {
                if (!model.DepartmentIds.Contains(departmentEmployee.DepartmentId))
                {
                    _context.DepartmentEmployee.Remove(departmentEmployee);
                }
            }

            if (model.DepartmentIds != null)
            {
                foreach (var departmentId in model.DepartmentIds)
                {
                    if (!employee.DepartmentEmployee.Any(du => du.DepartmentId == departmentId))
                    {
                        employee.DepartmentEmployee.Add(new DepartmentEmployee
                        {
                            EmployeeId = employee.Id,
                            DepartmentId = departmentId
                        });
                    }
                }
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();

                    var combinedContacts = model.Contacts.Concat(model.Relatives).ToList();
                    await _contactService.RemoveContacts("Employee", employee.Id, combinedContacts);

                    if (model.Contacts != null)
                        await _contactService.UpsertContacts("Employee", employee.Id, model.Contacts);

                    if(model.Relatives != null)
                        await _contactService.UpsertContacts("Employee", employee.Id, model.Relatives);

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

            return Ok(new { employee, message = "Saved successfully" });
        }

        [HttpGet("v1/employees/birthdays")]
        public async Task<Dictionary<string, List<IndexEmployeeViewModel>>> GetEmployeesWithBirthdaysAsync()
        {
            var employeesWithBirthdays = await _context.Employees
                .Where(e => e.InactiveDate == null)
                .OrderBy(e => e.Birthdate.Month)
                .ThenBy(e => e.Birthdate.Day)
                .Select(e => new IndexEmployeeViewModel
                {
                    Id = e.Id,
                    Name = e.FullName,
                    Birthdate = e.Birthdate.ToString("dd/MM")
                })
                .ToListAsync();

            var groupedByMonth = employeesWithBirthdays
                .GroupBy(e => e.Birthdate.Substring(3, 2))
                .ToDictionary(g => g.Key, g => g.ToList());

            return groupedByMonth;
        }
    }
}
