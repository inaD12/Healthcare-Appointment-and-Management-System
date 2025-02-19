using Shared.Domain.Events;
using Users.Application.Factories.Interfaces;

namespace Users.Application.Factories;

internal class UserConfirmEmailEventFactory : IUserConfirmEmailEventFactory
{
	public UserConfirmEmailEvent CreateUserConfirmEmailEvent(string email, string link)
	{
		return new UserConfirmEmailEvent(link, email);
	}
}