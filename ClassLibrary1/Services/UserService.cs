using Users.Infrastructure.Repositories;

namespace Users.Application.Services
{
	public class UserService
	{
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
