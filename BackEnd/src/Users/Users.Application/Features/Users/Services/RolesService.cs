using MediatR;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Users.Application.Features.Users.Queries.GetUserRoles;

namespace Users.Application.Features.Users.Services;

internal sealed class RolesService(ISender sender) : IRolesService
{
    public async Task<Result<RolesResponse>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await sender.Send(new GetUserRolesQuery(userId), cancellationToken);
    }
}
