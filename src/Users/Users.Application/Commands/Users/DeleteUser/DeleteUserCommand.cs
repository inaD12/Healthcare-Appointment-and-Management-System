using Contracts.Abstractions.Messaging;

namespace Users.Application.Commands.Users.DeleteUser;

public sealed record DeleteUserCommand(
	string Id) : ICommand;
