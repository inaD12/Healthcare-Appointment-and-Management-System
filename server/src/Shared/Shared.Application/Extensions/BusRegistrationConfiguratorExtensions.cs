using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Shared.Application.Extensions;

public static class BusRegistrationConfiguratorExtensions
{
	public static void AddTransactionalOutbox<TDbContext>(this IBusRegistrationConfigurator busConfigurator)
	   where TDbContext : DbContext
	{
		busConfigurator.AddEntityFrameworkOutbox<TDbContext>(o =>
		{
			o.QueryDelay = TimeSpan.FromSeconds(1);
			o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
			o.UsePostgres().UseBusOutbox();
		});
	}
}
