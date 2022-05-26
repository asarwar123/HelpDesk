using HelpDesk.api.DBContexts;
using HelpDesk.api.Entities;

namespace HelpDesk.api.Services
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataBaseContext _context;

        public AuthenticationRepository(DataBaseContext context)
        {
            _context = context ?? 
                            throw new ArgumentNullException(nameof(context));
        }
        public User? Authenticate(string userName, string password)
        {
            User? user = _context.Users.FirstOrDefault(u => u.UserName == userName &&
                                        u.Password == password);

            return user;
        }

        public Task<User> getUser(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
