using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ratings.Application.Features.RateableAppointments.Abstractions;
using Ratings.Application.Features.Ratings.Abstractions;
using Ratings.Infrastructure.Features.DBContexts;
using Ratings.Infrastructure.Features.Helpers;
using Ratings.Infrastructure.Features.Repositories;
using Shared.Application.Abstractions;
using Shared.Infrastructure.Extensions;

namespace Ratings.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		services
			.AddScoped<IDoctorRatingStatsRepository, DoctorRatingStatsRepository>()
			.AddScoped<IRateableAppointmentRepository, RateableAppointmentsRepository>()
			.AddScoped<IRatingRepository, RatingsRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<RatingsDbContext>()
			.AddMessageBroker(configuration, currentAssembly, "ratings")
			.AddAuth(configuration)
			.AddDateTimeProvider()
			.AddPermissionService()
			.AddDatabaseContext<RatingsDbContext>(configuration);
		
		return services;
	}
}
