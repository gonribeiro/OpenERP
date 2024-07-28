using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Auth;
using OpenERP.Services.Auth;
using OpenERP.ViewModels.Auth.Users;
using System.Data;
using System.Security.Claims;

namespace OpenERP.Controllers.v1.Auth
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly UserService _userService;

        public UserController(
            TokenService tokenService,
            AppDbContext context,
            RefreshTokenService refreshTokenService,
            UserService userService)
        {
            _tokenService = tokenService;
            _context = context;
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        [HttpPost("v1/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(model.Username);
                if (user == null)
                    return Unauthorized("User or password incorrect");

                bool verifyPassword = PasswordService.VerifyPassword(model.Password, user.Password);
                if (!verifyPassword)
                    return Unauthorized("User or password incorrect");

                await _refreshTokenService.RevokeRefreshTokensAsync(user.Id);

                var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(user, true);

                return Ok(new
                {
                    LastPasswordUpdatedAt = user.LastPasswordUpdatedAt.ToString(),
                    accessToken,
                    refreshToken
                });
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("v1/refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel model)
        {
            try
            {
                var newAccessToken = await _tokenService.RefreshTokenAsync(model.RefreshToken, model.UserId);

                if (newAccessToken == null) return Unauthorized();

                return Ok(new { newAccessToken });
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("v1/users")]
        [Authorize(Roles = "Admin")]
        public async Task<List<IndexUserViewModel>> GetUsersAsync()
        {
            var users = await _context.Users
                .OrderByDescending(c => c.Id)
                .Select(c => new IndexUserViewModel
                {
                    Id = c.Id,
                    InactiveDate = c.InactiveDate.HasValue ? c.InactiveDate.Value.ToString("yyyy-MM-dd") : null,
                    Employee = c.Employee.FullName,
                    Username = c.Username,
                    Roles = string.Join(", ", c.RoleUser.Select(ud => ud.Role.Name)),
                    LastPasswordUpdatedAt = c.LastPasswordUpdatedAt.HasValue
                        ? c.LastPasswordUpdatedAt.Value.ToString("yyyy-MM-dd")
                        : "Needs to update",
                })
                .ToListAsync();

            return users;
        }

        [HttpPost("v1/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserViewModel model)
        {
            try
            {
                var username = await _context.Users.FirstOrDefaultAsync(c => c.Username == model.Username);
                if (username != null)
                    return BadRequest("Username already exists");

                var employee = await _context.Employees
                    .Where(c => c.User == null && c.InactiveDate == null)
                    .FirstOrDefaultAsync(c => c.Id == model.EmployeeId);
                if (employee == null)
                    return BadRequest("Employee not found or inactive");

                var (isValid, errorMessage) = PasswordService.ValidatePassword(model.Password);
                if (!isValid)
                    return BadRequest(errorMessage);

                var password = PasswordService.HashPassword(model.Password);

                var user = new User
                {
                    EmployeeId = model.EmployeeId,
                    Username = model.Username,
                    Password = password,
                    CreatedAt = DateTime.UtcNow,
                };

                if (model.RoleIds != null)
                {
                    user.RoleUser = new List<RoleUser>();

                    foreach (var roleId in model.RoleIds)
                    {
                        var roleUser = new RoleUser
                        {
                            UserId = user.Id,
                            RoleId = roleId
                        };

                        user.RoleUser.Add(roleUser);
                    }
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return Ok(new
                {
                    user,
                    message = "Created successfully",
                    redirectTo = $"users/{user.Id}/edit"
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

        [HttpGet("v1/users/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var user = await _context
                    .Users
                    .Select(c => new GetUserViewModel
                    {
                        Id = c.Id,
                        InactiveDate = c.InactiveDate.HasValue ? c.InactiveDate.Value.ToString("yyyy-MM-dd") : null,
                        Employee = c.Employee.FullName,
                        Username = c.Username,
                        RoleIds = c.RoleUser.Select(ud => ud.Role.Id).ToList(),
                        LastPasswordUpdatedAt = c.LastPasswordUpdatedAt.HasValue
                            ? c.LastPasswordUpdatedAt.Value.ToString("yyyy-MM-dd")
                            : null,
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                    return NotFound("User not found");

                return Ok(user);
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

        [HttpPut("v1/users/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] UpdateUserViewModel model)
        {
            try
            {
                var user = await _context.Users.Include(u => u.RoleUser).FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                    return NotFound("User not found");

                var username = await _context.Users.FirstOrDefaultAsync(c => c.Id != id && c.Username == model.Username);
                if (username != null)
                    return BadRequest("Username already exists");

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (model.InactiveDate != null && user.Id.ToString() == loggedInUserId)
                    return BadRequest("You cannot deactivate the user account that you are currently using.");

                user.Username = model.Username;
                user.InactiveDate = model.InactiveDate;

                if (model.InactiveDate != null)
                    await _refreshTokenService.RevokeRefreshTokensAsync(user.Id);

                if (model.Password != null)
                {
                    var (isValid, errorMessage) = PasswordService.ValidatePassword(model.Password);
                    if (!isValid)
                        return BadRequest(errorMessage);

                    var password = PasswordService.HashPassword(model.Password);
                    user.Password = password;

                    user.LastPasswordUpdatedAt = user.Id.ToString() == loggedInUserId
                        ? DateTime.UtcNow
                        : null;
                }

                foreach (var roleId in model.RoleIds)
                {
                    if (!user.RoleUser.Any(du => du.RoleId == roleId))
                    {
                        user.RoleUser.Add(new RoleUser
                        {
                            UserId = user.Id,
                            RoleId = roleId
                        });
                    }
                }

                foreach (var roleUser in user.RoleUser.ToList())
                {
                    if (!model.RoleIds.Contains(roleUser.RoleId))
                    {
                        _context.RoleUser.Remove(roleUser);
                    }
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return Ok(new { user, message = "Saved successfully" });
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

        [HttpPut("v1/users/updatePassword")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordViewModel model)
        {
            try
            {
                var (isValid, errorMessage) = PasswordService.ValidatePassword(model.Password);
                if (!isValid)
                    return BadRequest(errorMessage);

                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == loggedInUserId);

                if (user == null)
                    return NotFound("User not found");

                var newPassword = PasswordService.HashPassword(model.Password);
                user.Password = newPassword;
                user.LastPasswordUpdatedAt = DateTime.UtcNow;

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return Ok(new
                {
                    user,
                    message = "Created successfully",
                    redirectTo = $"users/{user.Id}/edit"
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
