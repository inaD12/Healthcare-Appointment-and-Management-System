using Shared.Domain.Models;

namespace Shared.Domain.Abstractions;

public interface IBatchNamesService
{
    Task<GetUsersByIdsResponse> GetUsersNamesByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
}