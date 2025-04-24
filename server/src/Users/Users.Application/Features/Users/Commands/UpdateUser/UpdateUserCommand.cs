using Shared.Domain.Abstractions.Messaging;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(
	string Id,
	string? NewEmail,
	string? FirstName,
	string? LastName) : ICommand<UserCommandViewModel>;
