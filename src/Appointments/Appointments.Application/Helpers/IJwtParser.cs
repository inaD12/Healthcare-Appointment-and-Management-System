﻿using Shared.Domain.Results;

namespace Appointments.Application.Helpers;

public interface IJwtParser
{
	Result<string> GetIdFromToken();
}