using Appointments.Domain.Entities;
using Shared.Domain.Enums;

namespace Appointments.Application.Features.Appointments.Factories.Abstractions;

public interface IUserDataFactory
{
	UserData Create(string userId, string email, Roles role);
}