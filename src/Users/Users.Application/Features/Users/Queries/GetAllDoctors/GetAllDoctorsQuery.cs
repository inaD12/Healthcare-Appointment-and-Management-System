using Shared.Domain.Abstractions.Messaging;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Queries.GetAllDoctors;

public sealed record GetAllDoctorsQuery() : IQuery<IEnumerable<User>>;
