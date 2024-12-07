using Appointments.Infrastructure.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Appointments.Infrastructure.Schedulers
{
	public static class HangfireJobScheduler
	{
		public static void ConfigureJobs(IApplicationBuilder app)
		{
			RecurringJob.AddOrUpdate<UseCaseExecutor>(
				"MarkAppointmentsAsCompleted",
				job => job.ExecuteAsync(),
				Cron.Hourly); // Adjust schedule as needed
		}
	}
}
