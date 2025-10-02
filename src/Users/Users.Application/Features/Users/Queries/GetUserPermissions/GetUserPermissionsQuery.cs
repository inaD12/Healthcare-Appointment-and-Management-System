using Shared.Application.Authorization;
using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Features.Users.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
