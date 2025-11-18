using Appointments.Domain.Entities;
using Shared.Application.IntegrationEvents;

namespace Appointments.Application.Features.Mappers;

public static class Mapper
{
    public static UserData ToUserData(
        this UserCreatedIntegrationEvent intEvent)
        => new(
            intEvent.Id,
            intEvent.Email,
            intEvent.Roles.ToList());
}