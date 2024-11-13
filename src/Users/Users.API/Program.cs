using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.UsersDBContexts;
using Users.Application;
using Users.Infrastructure;
using UsersAPI.Extentions;
using Serilog;
using UsersAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Healthcare_Appointment_and_Management_System.Extentions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddDbContext<UsersDBContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDBConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAppSettings(config);
builder.Services.InjectAuthentication(config);
builder.Services.AddSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.InjectMassTransit();

builder.Services
	.AddApplicationLayer(config)
	.AddInfrastructureLayer();

builder.Host.ConfigureSerilog();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = false;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigrations();
}

app.UseSerilogRequestLogging();


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

EndpointMapper.MapAllEndpoints(app);

//app.UseHttpsRedirection();

app.Run();
