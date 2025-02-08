using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Commands.Email.HandleEmail;

public sealed record HandleEmailCommand(
	string tokenId) : ICommand;
