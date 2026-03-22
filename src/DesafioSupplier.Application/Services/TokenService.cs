using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DesafioSupplier.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using DesafioSupplier.Application.Auth;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class TokenService(IOptions<AuthSettings> authSettings) : ITokenService
{
    public Task<string> GetTokenAsync(User user)
    {
        var expirationTime = DateTime.UtcNow.AddHours(authSettings.Value.TokenDurationInHours);

        var claims = new[]
        {
            new Claim("Id", user.Id),
            new Claim("Username", user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, "DesafioSupplierAuth"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Value.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: authSettings.Value.Issuer,
            audience: authSettings.Value.Audience,
            claims: claims,
            expires: expirationTime,
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}
