using Users.Domain.Entities;
using Users.Domain.Result;

namespace Users.Infrastructure.Repositories
{
	public interface IUserRepository
	{
		Result<IEnumerable<User>> GetAllUsers();
		Result<User> GetUserById(int id);
		Result<User> GetUserByEmail(string email);
		Result<User> GetUserByFirstName(string FirstName);
		void AddUser(User user);
		void UpdateUser(User user);
		void DeleteUser(int id);
	}
}
