using Users.Application.Features.Email.Models;

namespace Users.Application.Features.Email.Helpers.Abstractions;

public interface IEmailConfirmationTokenPublisher
{
	Task PublishEmailConfirmationTokenAsync(PublishEmailConfirmationTokenModel model);
}