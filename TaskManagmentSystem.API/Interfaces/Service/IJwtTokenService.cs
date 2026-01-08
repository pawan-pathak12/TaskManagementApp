using TaskManagmentSystem.API.Entities;

namespace TaskManagmentSystem.API.Interfaces.Service
{
    public interface IJwtTokenService
    {
        string CreateToken(User user);
    }

}
