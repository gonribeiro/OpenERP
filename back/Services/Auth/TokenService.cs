using Microsoft.IdentityModel.Tokens;
using OpenERP.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenERP.Services.Auth
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly UserService _userService;

        public TokenService(
            IConfiguration configuration,
            RefreshTokenService refreshTokenService,
            UserService userService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        public async Task<(string AccessToken, string? RefreshToken)> CreateTokenAsync(User user, bool withRefreshToken = false)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddMinutes(15),
                Subject = GenerateClaims(user),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = handler.CreateToken(tokenDescriptor);
            var accessToken = handler.WriteToken(token);

            string? refreshToken = null;
            if (withRefreshToken)
                refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return (accessToken, refreshToken);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var ci = new ClaimsIdentity();
            var givenName = user.Employee != null
                ? user.Employee.FirstName
                : user.Username;

            ci.AddClaim(new Claim("id", user.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, givenName));

            foreach (var role in user.RoleUser)
                ci.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));

            return ci;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken, int userId)
        {
            if (await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken))
            {
                var user = await _userService.GetUserByIdAsync(userId);
                return (await CreateTokenAsync(user)).AccessToken;
            }

            return null;
        }
    }
}