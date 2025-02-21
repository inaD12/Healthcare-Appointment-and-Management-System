using Shared.Domain.Abstractions.Messaging;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Users.Queries.GetAllDoctors;

public sealed record GetAllDoctorsQuery() : IQuery<IEnumerable<UserResponseDTO>>;
