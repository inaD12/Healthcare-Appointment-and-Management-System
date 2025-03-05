using Shared.Domain.Abstractions.Messaging;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.LoginUser;

public sealed record LoginUserCommand(
	string Email,
	string Password) : ICommand<LoginUserCommandViewModel>;
