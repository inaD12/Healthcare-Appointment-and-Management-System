namespace Users.Application.Features.Email.Helpers.Abstractions;

public interface IEmailConfirmationTokenPublisher
{
	Task PublishEmailConfirmationTokenAsync(string email, string userId);
}