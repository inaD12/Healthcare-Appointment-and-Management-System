using Shared.Application.Authorization;
using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Features.Users.Commands.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
