using Appointments.Application.Jobs;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Appointments.Application.Helpers;

public class HangfireHostedService : IHostedService
{
	private readonly IRecurringJobManager _recurringJobManager;
	private readonly IConfiguration configuration;

	public HangfireHostedService(IRecurringJobManager recurringJobManager, IConfiguration configuration)
	{
		_recurringJobManager = recurringJobManager;
		this.configuration = configuration;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		GlobalConfiguration.Configuration
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("AppointmentsDBConnection")));

		_recurringJobManager.AddOrUpdate<CompleteAppointmentsJob>("CompleteAppointmentsJob", job => job.Execute(cancellationToken), Cron.Minutely);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
