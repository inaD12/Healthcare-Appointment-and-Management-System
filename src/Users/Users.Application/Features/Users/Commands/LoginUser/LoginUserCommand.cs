using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Features.Users.LoginUser;

public sealed record LoginUserCommand<TokenResult>(
	string Email, string Password) : ICommand<TokenResult>;
