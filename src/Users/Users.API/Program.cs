using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.UsersDBContexts;
using Users.Application;
using Users.Infrastructure;
using Healthcare_Appointment_and_Management_System.Extentions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddDbContext<UsersDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDBConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAppSettings(config);
builder.Services.InjectAuthentication(config);

builder.Services.AddSwagger();

builder.Services.AddHttpContextAccessor();

builder.Services
	.AddApplicationLayer()
	.AddInfrastructureLayer();

builder.Host.ConfigureSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
EndpointMapper.MapAllEndpoints(app);

app.Run();
