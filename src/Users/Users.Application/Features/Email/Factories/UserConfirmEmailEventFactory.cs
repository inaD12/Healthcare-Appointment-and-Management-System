using Shared.Domain.Events;
using Users.Application.Features.Email.Factories.Abstractions;

namespace Users.Application.Features.Email.Factories;

internal class UserConfirmEmailEventFactory : IUserConfirmEmailEventFactory
{
	public UserConfirmEmailEvent CreateUserConfirmEmailEvent(string email, string link)
	{
		return new UserConfirmEmailEvent(link, email);
	}
}