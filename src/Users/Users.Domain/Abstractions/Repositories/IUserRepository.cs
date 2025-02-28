using Shared.Domain.Abstractions;
using Shared.Domain.Results;
using Users.Domain.Entities;

namespace Users.Domain.Abstractions.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
	Task<Result<IEnumerable<User>>> GetAllDoctorsAsync();
	Task<Result<User>> GetByEmailAsync(string email);
	Task<Result<User>> GetByFirstNameAsync(string firstName);
	Task<Result> DeleteByIdAsync(string id);
	void VerifyEmailAsync(User user);
}
