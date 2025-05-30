using Shared.Domain.Abstractions.Messaging;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.Queries.GetById;

public sealed record GetUserByIdQuery(string Id) : IQuery<UserQueryViewModel>;
