using Appointments.Domain.Entities;

namespace Appointments.Application.Factories
{
	public interface IUserDataFactory
	{
		UserData Create(string userId, string email, string role);
	}
}