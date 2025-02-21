using Shared.Domain.Events;

namespace Users.Application.Features.Email.Factories.Abstractions
{
	public interface IUserConfirmEmailEventFactory
	{
		UserConfirmEmailEvent CreateUserConfirmEmailEvent(string email, string link);
	}
}