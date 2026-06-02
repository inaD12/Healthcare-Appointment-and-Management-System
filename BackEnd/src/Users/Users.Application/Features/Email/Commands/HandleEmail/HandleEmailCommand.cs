using Shared.Application.Abstractions.Messaging;

namespace Users.Application.Features.Email.Commands.HandleEmail;

public sealed record HandleEmailCommand(
	string TokenId) : ICommand;
