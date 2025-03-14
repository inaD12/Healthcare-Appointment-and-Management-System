﻿using Appointments.Infrastructure.Features.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Appointments.Infrastructure.Features.Schedulers;

public static class HangfireJobScheduler
{
	public static void ConfigureJobs(IApplicationBuilder app)
	{
		RecurringJob.AddOrUpdate<UseCaseExecutor>(
			"MarkAppointmentsAsCompleted",
			job => job.ExecuteAsync(),
			Cron.Hourly);
	}
}
