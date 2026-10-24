using Application.DTO;
using DB.Repositories.Interfaces;
using Domain.Class;

namespace Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenCreator _tokenCreator;

    public UserService(IUserRepository userRepository, ITokenCreator tokenCreator)
    {
        _userRepository = userRepository;
        _tokenCreator = tokenCreator;
    }

    public async Task<string> AuthentificateAsync(AuthDto dto)
    {
        User user = await GetVerifiedUserAsync(dto);

        return _tokenCreator.CreateToken(user);
    }

    public async Task<bool> CreateUserAsync(CreateUserDto dto)
    {
        if (dto is null) throw new Exception("UserDataIsEmpty");

        string username = dto.Name?.Trim() ?? string.Empty;
        if (username.Length is 0 or > 128) throw new Exception("UserNameIsInvalid");
        if (string.IsNullOrWhiteSpace(dto.Password)) throw new Exception("UserPasswordIsEmpty");

        if (!Enum.TryParse(dto.Role, ignoreCase: true, out UserRole role) || !Enum.IsDefined(role))
            throw new Exception("UserRoleIsUnknown");

        User? existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser is not null) throw new Exception("UserAlreadyExists");

        User user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role
        };

        await _userRepository.AddAsync(user);

        return true;
    }

    public async Task<List<string>> GetUserRolesAsync(AuthDto dto)
    {
        User user = await GetVerifiedUserAsync(dto);

        return new List<string> { user.Role.ToString() };
    }

    private async Task<User> GetVerifiedUserAsync(AuthDto dto)
    {
        if (dto is null) throw new Exception("AuthDataIsEmpty");
        if (string.IsNullOrWhiteSpace(dto.Login)) throw new Exception("LoginIsEmpty");
        if (string.IsNullOrWhiteSpace(dto.Password)) throw new Exception("PasswordIsEmpty");

        User? user = await _userRepository.GetByUsernameAsync(dto.Login.Trim());
        if (user is null) throw new Exception("UserNotFound");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isPasswordValid) throw new Exception("PasswordIsIncorrect");

        return user;
    }
}
