using Application.DTO;

namespace Application.Service;

public interface IUserService
{
    Task<string> AuthentificateAsync(AuthDto dto);
    Task<bool> CreateUserAsync(CreateUserDto dto);
    Task<List<string>> GetUserRolesAsync(AuthDto dto);
}
