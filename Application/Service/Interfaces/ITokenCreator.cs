using Domain.Class;

namespace Application.Service;

public interface ITokenCreator
{
    string CreateToken(User user);
}
