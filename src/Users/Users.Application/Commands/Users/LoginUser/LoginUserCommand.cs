using Contracts.Abstractions.Messaging;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Commands.Users.LoginUser;

public sealed record LoginUserCommand<TokenDTO>(
	string email, string password) : ICommand<TokenDTO>;
