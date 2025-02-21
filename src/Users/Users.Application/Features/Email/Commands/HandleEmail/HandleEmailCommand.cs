using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Features.Email.Commands.HandleEmail;

public sealed record HandleEmailCommand(
	string tokenId) : ICommand;
