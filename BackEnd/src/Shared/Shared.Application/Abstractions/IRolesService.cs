using Shared.Application.Models;
using Shared.Domain.Results;

namespace Shared.Application.Abstractions;

public interface IRolesService
{
    Task<Result<RolesResponse>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
}
