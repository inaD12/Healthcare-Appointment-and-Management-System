using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Features.Users.LoginUser;

public sealed record LoginUserCommand<TokenDTO>(
	string Email, string Password) : ICommand<TokenDTO>;
