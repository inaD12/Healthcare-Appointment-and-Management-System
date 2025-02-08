using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Commands.Email.SendEmail;

public sealed record SendEmailCommand(
	string userId, string userEmail) : ICommand;

