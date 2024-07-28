using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Models.Auth;
using System.Security.Cryptography;

public class RefreshTokenService
{
    private readonly AppDbContext _context;

    public RefreshTokenService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateRefreshTokenAsync(int userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = userId,
            ExpiryDate = DateTime.UtcNow.AddHours(12)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
    {
        var token = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken && rt.InactiveDate == null);

        return token != null && token.ExpiryDate > DateTime.UtcNow;
    }

    public async Task RevokeRefreshTokensAsync(int userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.InactiveDate == null)
            .ToListAsync();

        if (tokens.Any())
        {
            foreach (var token in tokens)
            {
                token.InactiveDate = DateTime.UtcNow;
            }

            _context.RefreshTokens.UpdateRange(tokens);
            await _context.SaveChangesAsync();
        }
    }
}