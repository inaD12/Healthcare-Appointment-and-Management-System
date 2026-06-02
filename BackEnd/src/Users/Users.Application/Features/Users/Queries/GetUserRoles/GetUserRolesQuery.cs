using Shared.Application.Abstractions.Messaging;
using Shared.Application.Models;

namespace Users.Application.Features.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(string UserId) : IQuery<RolesResponse>;
