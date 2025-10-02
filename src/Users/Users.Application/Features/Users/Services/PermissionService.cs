using MediatR;
using Shared.Application.Authorization;
using Shared.Domain.Results;
using Users.Application.Features.Users.Queries.GetUserPermissions;

namespace Users.Application.Features.Users.Services;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
