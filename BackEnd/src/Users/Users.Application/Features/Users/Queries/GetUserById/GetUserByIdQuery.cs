using Shared.Domain.Abstractions.Messaging;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(string Id) : IQuery<UserQueryViewModel>;
