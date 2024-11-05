using Appointments.API.Extentions;
using Appointments.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwagger();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.Run();

