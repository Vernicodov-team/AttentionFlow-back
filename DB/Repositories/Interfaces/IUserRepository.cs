using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByUsernameAsync(string username);

    Task<List<User>> GetAllAsync();

    Task<User> UpdateAsync(User user);
}
