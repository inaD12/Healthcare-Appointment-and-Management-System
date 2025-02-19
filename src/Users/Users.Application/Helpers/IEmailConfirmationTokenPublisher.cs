namespace Users.Application.Helpers;

public interface IEmailConfirmationTokenPublisher
{
	Task PublishEmailConfirmationTokenAsync(string email, string userId);
}