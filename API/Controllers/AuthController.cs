using System.IdentityModel.Tokens.Jwt;
using API.Modules;
using Application.DTO;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/auth")]

public class AuthController: ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(AuthDto dto)
    {
        var answ = await _userService.AuthentificateAsync(dto);
        return Ok(answ);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMeAsync()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var username = User.FindFirst(JwtClaimNames.Name)?.Value;
        var role = User.FindFirst(JwtClaimNames.Role)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Не удалось определить пользователя из токена");
        }

        return Ok(new
        {
            Id = userId,
            Username = username,
            Role = role
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        return Ok(new { message = "Успешный выход" });
    }
}