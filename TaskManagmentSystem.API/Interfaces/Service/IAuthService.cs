using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Results;
namespace TaskManagmentSystem.API.Interfaces.Service
{
    public interface IAuthService

    {
        Task<AuthResult> LoginAsync(User user);
        Task<AuthResult> RegisterAsync(User user);
    }
}
