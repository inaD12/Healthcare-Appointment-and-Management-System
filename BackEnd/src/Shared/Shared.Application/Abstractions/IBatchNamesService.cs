using Shared.Application.Models;

namespace Shared.Application.Abstractions;

public interface IBatchNamesService
{
    Task<GetUsersByIdsResponse> GetUsersNamesByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);
}