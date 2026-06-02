using Shared.Application.Abstractions.Messaging;

namespace Users.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(
	string Id) : ICommand;
