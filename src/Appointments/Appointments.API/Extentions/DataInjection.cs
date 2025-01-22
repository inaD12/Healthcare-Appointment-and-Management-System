using Appointments.Application.Consumers;
using Appointments.Application.Settings;
using Appointments.Domain.Enums;
using Appointments.Infrastructure.DBContexts;
using Contracts.Enums;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace Appointments.API.Extentions
{
	public static class DataInjection
	{
		public static IServiceCollection InjectAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opts =>
				{
					byte[] signingKeyBytes = Encoding.UTF8
						.GetBytes(configuration["Auth:SecretKey"]);

					opts.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration["Auth:Issuer"],
						ValidAudience = configuration["Auth:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:SecretKey"]))
					};
				});

			services.AddAuthorization();

			return services;
		}

		public static IServiceCollection InjectMassTransit(this IServiceCollection services)
		{
			services.AddMassTransit(busConfiguratior =>
			{
				busConfiguratior.AddEntityFrameworkOutbox<AppointmentsDBContext>(o =>
				{
					o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
					o.UsePostgres();
				});

				busConfiguratior.SetKebabCaseEndpointNameFormatter();

				busConfiguratior.AddConsumer<UserCreatedConsumer>();

				busConfiguratior.UsingRabbitMq((context, configurator) =>
				{
					MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

					configurator.Host(new Uri(settings.Host), h =>
					{
						h.Username(settings.Username);
						h.Password(settings.Password);
					});

					configurator.ConfigureEndpoints(context);
				});
			});

			return services;
		}
		public static void ConfigureSerilog(this IHostBuilder hostBuilder)
		{
			hostBuilder.UseSerilog((context, configuration) =>
				configuration
					.ReadFrom.Configuration(context.Configuration)
					.Enrich.FromLogContext()
			);
		}

		public static IServiceCollection ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHangfire(config =>
			{
				config.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString("AppointmentsDBConnection")));
			});

			services.AddHangfireServer();

			return services;
		}

		public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<MessageBrokerSettings>(
				configuration.GetSection("MessageBroker"));

			return services;
		}

		public static IServiceCollection ConfigureDBs(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppointmentsDBContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("AppointmentsDBConnection"), o =>
		   {
			   o.MapEnum<Roles>("roles");

			   o.MapEnum<AppointmentStatus>("appointmentstatus");
		   }));

			return services;
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter JWT Bearer token **_only_**",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};
				options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{securityScheme, new string[] { }}
			});
			});

			return services;
		}
	}
}
