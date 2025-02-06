using Microsoft.Extensions.DependencyInjection;
using Shared.Application.PipelineBehaviors;
using System.Reflection;

namespace Shared.Application.Extensions;

public static class ServiceCollectionExtentions
{
	public static IServiceCollection AddMediatR(this IServiceCollection serviceCollection, Assembly assembly)
	{
		serviceCollection.AddMediatR(
			cf =>
			{
				cf.RegisterServicesFromAssembly(assembly);

				cf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			});

		return serviceCollection;
	}
}
