using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Users.Application.Settings;

namespace UsersAPI.Extentions
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
				busConfiguratior.SetKebabCaseEndpointNameFormatter();

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

		public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AuthValues>(
				configuration.GetSection("Auth"));
			services.Configure<ConnectionStrings>(
				configuration.GetSection("ConnectionStrings"));
			services.Configure<MessageBrokerSettings>(
				configuration.GetSection("MessageBroker"));

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
