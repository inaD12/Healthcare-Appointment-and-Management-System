using Shared.Application.IntegrationEvents;
using Shared.Domain.Extensions;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Mappers;

public static class CommandMapper
{
    public static UserCreatedIntegrationEvent ToIntEvent(
        this User user)
        => new(
            user.Id,
            user.Email,
            user.Roles.Select(r => r.MapToRoleEnum()).ToList());
    
    public static UserCommandViewModel ToCommandViewModel(
        this User user)
        => new(
            user.Id);
}
  