using Shared.Application.Abstractions;
using Shared.Application.Models;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Abstractions;

public interface IUserRepository : IGenericRepository<User>
{
	Task<PagedList<User>?> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken = default);
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<Dictionary<string, NamesResponse>> GetNamesByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
	Task<NamesResponse?> GetNameByIdAsync(string userId, CancellationToken cancellationToken = default);
}
