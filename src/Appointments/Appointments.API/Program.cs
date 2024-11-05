using Appointments.API.Extentions;
using Appointments.API.Middlewares;
using Appointments.Application.DependancyInjection;
using Appointments.Infrastructure.DBContexts;
using Appointments.Infrastructure.DependancyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

//builder.Services.AddDbContext<AppointmentsDBContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentsDBConnection")));

builder.Services.AddDbContext<UserDataDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDataDBConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwagger();

builder.Services
	.AddApplicationLayer(config)
	.AddInfrastructureLayer(config);

builder.Host.ConfigureSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.Run();

