using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.IntegrationTests.Extentensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTestDbContext<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder>? optionsAction = null)
	  where TContext : DbContext
	{
		var efCoreDescriptor = serviceCollection.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<TContext>));

		if (efCoreDescriptor != null)
		{
			serviceCollection.Remove(efCoreDescriptor);
		}

		serviceCollection.AddDbContext<TContext>(options => optionsAction?.Invoke(options));

		return serviceCollection;
	}
}