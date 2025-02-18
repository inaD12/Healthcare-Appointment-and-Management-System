using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.Domain.Options;
using System.Text;

namespace Shared.API.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddOptions<AuthOptions>()
			.BindConfiguration(nameof(AuthOptions))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		var tokenOptions = configuration
			.GetSection(nameof(AuthOptions))
			.Get<AuthOptions>()!;

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(opts =>
			{
				byte[] signingKeyBytes = Encoding.UTF8
					.GetBytes(tokenOptions.SecretKey);

				opts.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
				};
			});

		services.AddAuthorization();

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

	public static IServiceCollection ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("AllowAllOrigins", builder =>
			{
				builder.AllowAnyOrigin()
					   .AllowAnyMethod()
					   .AllowAnyHeader();
			});
		});

		return services;
	}
}
