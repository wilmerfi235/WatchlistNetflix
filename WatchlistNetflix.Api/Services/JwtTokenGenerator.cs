using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace WatchlistNetflix.Api.Services;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly SymmetricSecurityKey _signingKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _lifetime;

    public JwtTokenGenerator(
        SymmetricSecurityKey signingKey,
        string issuer,
        string audience,
        TimeSpan lifetime)
    {
        ArgumentNullException.ThrowIfNull(signingKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(audience);
        if (lifetime <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(lifetime));
        }

        _signingKey = signingKey;
        _issuer = issuer;
        _audience = audience;
        _lifetime = lifetime;
    }

    public string GenerateToken(Guid userId, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var now = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };
        var credentials = new SigningCredentials(
            _signingKey,
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            now,
            now.Add(_lifetime),
            credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
