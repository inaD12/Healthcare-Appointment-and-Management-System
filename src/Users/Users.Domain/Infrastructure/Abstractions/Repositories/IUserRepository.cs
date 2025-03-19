using Shared.Domain.Abstractions;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Models;

namespace Users.Domain.Infrastructure.Abstractions.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
	Task<Result<PagedList<User>>> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken);
	Task<Result<User>> GetByEmailAsync(string email);
	Task<Result> DeleteByIdAsync(string id);
}
