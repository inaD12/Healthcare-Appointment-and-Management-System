using Shared.Domain.Models;
using Shared.Domain.Results;

namespace Shared.Domain.Abstractions;

public interface IRolesService
{
    Task<Result<RolesResponse>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
}
