using Shared.Domain.Abstractions.Messaging;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Queries.Users.GetAllDoctors;

public sealed record GetAllDoctorsQuery() : IQuery<IEnumerable<UserResponseDTO>>;
