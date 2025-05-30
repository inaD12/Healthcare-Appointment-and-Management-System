﻿using Appointments.Domain.Entities.Enums;
using Shared.Domain.Exceptions;

namespace Appointments.Domain.Entities.ValueObjects;

public sealed record DateTimeRange
{
	private DateTimeRange()
	{
	}

	public DateTime Start { get; init; }

	public DateTime End { get; init; }

	public static DateTimeRange Create(DateTime start, DateTime end)
	{
		if (start > end)
		{
			throw new HAMSValidationException("EndTime" ,"End date precedes start date");
		}

		return new DateTimeRange
		{
			Start = start.ToUniversalTime(),
			End = end.ToUniversalTime()
		};
	}

	public static DateTimeRange Create(DateTime start, AppointmentDuration duration)
	{
		DateTime end = start.AddMinutes((int)duration);

		return Create(start, end);
	}
}
