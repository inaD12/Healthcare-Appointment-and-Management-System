using Shared.Domain.Abstractions;
using Shared.Domain.Models;
using Users.Domain.Entities;
using Users.Domain.Models;

namespace Users.Domain.Abstractions.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
	Task<PagedList<User>?> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken = default);
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
