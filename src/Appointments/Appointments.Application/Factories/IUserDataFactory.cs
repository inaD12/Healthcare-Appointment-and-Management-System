using Appointments.Domain.Entities;
using Contracts.Enums;

namespace Appointments.Application.Factories
{
	public interface IUserDataFactory
	{
		UserData Create(string userId, string email, Roles role);
	}
}