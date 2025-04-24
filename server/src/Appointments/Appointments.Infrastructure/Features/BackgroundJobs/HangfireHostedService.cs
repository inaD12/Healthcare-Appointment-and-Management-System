using Appointments.Domain.Infrastructure.Abstractions;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared.Domain.Options;

namespace Appointments.Infrastructure.Features.BackgroundJobs;

public class HangfireHostedService : IHostedService
{
	private readonly IRecurringJobManager _recurringJobManager;
	private readonly IConfiguration configuration;

	public HangfireHostedService(IRecurringJobManager recurringJobManager, IConfiguration configuration)
	{
		_recurringJobManager = recurringJobManager;
		this.configuration = configuration;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		var dbOptions = configuration
			.GetSection(nameof(DatabaseOptions))
			.Get<DatabaseOptions>()!;

		GlobalConfiguration.Configuration
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(dbOptions.ConnectionString));

		_recurringJobManager.AddOrUpdate<ICompleteAppointmentsJob>("CompleteAppointmentsJob", job => job.Execute(cancellationToken), Cron.Minutely);

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
