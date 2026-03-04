using Doctors.Domain.Abstractions;
using Doctors.Domain.Abstractions.Repositories;
using Doctors.Domain.Entities;
using Doctors.Domain.Options;
using Doctors.Infrastructure.Features.Clients;
using Doctors.Infrastructure.Features.DBContexts;
using Doctors.Infrastructure.Features.Helpers;
using Doctors.Infrastructure.Features.Repositories;
using Doctors.Infrastructure.Features.Seed;
using Doctors.Infrastructure.Features.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions;
using Shared.Infrastructure.Extensions;

namespace Doctors.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddScoped<IDoctorRepository, DoctorRepository>()
			.AddScoped<ISpecialityRepository, SpecialityRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>()
			.AddTransient<INamesService, NamesService>();

		services
			.AddUnitOfWork<DoctorsDbContext>()
			.AddAuth(configuration)
			.AddPermissionService()
			.AddMessageBroker(configuration, currentAssembly, busConfigurator =>
			{
				busConfigurator.AddTransactionalOutbox<DoctorsDbContext>();
			})
			.AddDatabaseContext<DoctorsDbContext>(configuration, optionsAction =>
			{
				optionsAction.UseVector();
				optionsAction.MapEnum<DayOfWeek>("dayofweek");
				optionsAction.MapEnum<AvailabilityExceptionType>("availabilityexceptiontype");
			})
			.AddHostedService<SpecialitySeederHostedService>();;
		
		services
			.AddOptions<OllamaOptions>()
			.BindConfiguration(nameof(OllamaOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		services.AddHttpClient<IEmbeddingClient, EmbeddingClient>(client =>
		{
			var settings = configuration
				.GetSection(nameof(OllamaOptions))
				.Get<OllamaOptions>()!;
		
			client.BaseAddress = new Uri(settings.Url);
		});
		
		return services;
	}
}
