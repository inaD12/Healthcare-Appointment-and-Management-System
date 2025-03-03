﻿using Shared.Domain.Abstractions.Messaging;

namespace Users.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(
	string Id,
	string? NewEmail,
	string? FirstName,
	string? LastName) : ICommand;
