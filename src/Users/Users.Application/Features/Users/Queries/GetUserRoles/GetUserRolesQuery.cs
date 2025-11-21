using Shared.Application.Dtos;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Models;

namespace Users.Application.Features.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(string UserId) : IQuery<RolesResponse>;
