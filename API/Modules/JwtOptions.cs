using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Modules;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int LifetimeHours { get; set; } = 8;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer)) throw new Exception("JwtIssuerIsMissing");
        if (string.IsNullOrWhiteSpace(Audience)) throw new Exception("JwtAudienceIsMissing");
        if (string.IsNullOrWhiteSpace(Key)) throw new Exception("JwtKeyIsMissing");
        if (Encoding.UTF8.GetByteCount(Key) < 32) throw new Exception("JwtKeyIsTooShort");

        if (LifetimeHours <= 0) throw new Exception("JwtLifetimeIsInvalid");
    }

    public SymmetricSecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

    public TokenValidationParameters CreateValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSecurityKey(),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = JwtClaimNames.Name,
            RoleClaimType = JwtClaimNames.Role
        };
    }
}

public static class JwtClaimNames
{
    public const string Name = "name";
    public const string Role = "role";
}
