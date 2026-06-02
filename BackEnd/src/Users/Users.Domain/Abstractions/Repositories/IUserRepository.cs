using Shared.Application.Abstractions;
using Shared.Application.Models;
using Users.Domain.Entities;
using Users.Domain.Models;

namespace Users.Domain.Abstractions.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
	Task<PagedList<User>?> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken = default);
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<Dictionary<string, NamesResponse>> GetNamesByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
}
