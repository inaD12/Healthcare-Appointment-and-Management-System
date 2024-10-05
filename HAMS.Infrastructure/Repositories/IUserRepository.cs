using Users.Domain.Entities;

namespace Users.Infrastructure.Repositories
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<User> GetUserByEmailAsync(string email);
		Task AddUserAsync(User user);
		Task UpdateUserAsync(User user);
		Task DeleteUserAsync(int id);
	}
}
