using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Interfaces.Service
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }

}
