using Contracts.Abstractions.Messaging;

namespace Users.Application.Commands.Users.LoginUser;

public sealed record LoginUserCommand<TokenDTO>(
	string Email, string Password) : ICommand<TokenDTO>;
