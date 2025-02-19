using Shared.Domain.Events;

namespace Users.Application.Factories.Interfaces
{
	public interface IUserConfirmEmailEventFactory
	{
		UserConfirmEmailEvent CreateUserConfirmEmailEvent(string email, string link);
	}
}