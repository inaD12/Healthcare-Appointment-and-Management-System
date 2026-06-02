using Shared.Application.Abstractions.Messaging;
using Shared.Application.Authorization;

namespace Users.Application.Features.Users.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
