using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Service;
using Domain.Class;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Modules;

internal static class JWTConfigurer
{
    internal static void SetupJWT(this WebApplicationBuilder builder)
    {
        JwtOptions options = builder.Configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>() ?? new JwtOptions();
        
        options.Validate();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

        builder.Services.AddAuthentication(jwtOptions =>
            {
                jwtOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                jwtOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.MapInboundClaims = false;
                jwtOptions.TokenValidationParameters = options.CreateValidationParameters();
            });

        builder.Services.AddAuthorization();

        builder.Services.AddSingleton<ITokenCreator, JWTCreator>();
    }
}

public class JWTCreator : ITokenCreator
{
    private readonly JwtOptions _options;

    public JWTCreator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _options.Validate();
    }

    public string CreateToken(User user)
    {
        if (user is null) throw new Exception("UserIsMissing");
        if (string.IsNullOrWhiteSpace(user.Username)) throw new Exception("UserNameIsMissing");

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtClaimNames.Name, user.Username),
            new Claim(JwtClaimNames.Role, user.Role.ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(_options.LifetimeHours),
            signingCredentials: new SigningCredentials(_options.GetSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
