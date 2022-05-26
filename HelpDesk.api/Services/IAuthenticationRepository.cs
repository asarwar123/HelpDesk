using HelpDesk.api.Entities;

namespace HelpDesk.api.Services
{
    public interface IAuthenticationRepository
    {
        User? Authenticate(string userName,string password);
        Task<bool> Logout();

        Task<User> getUser(string userName);
    }
}
