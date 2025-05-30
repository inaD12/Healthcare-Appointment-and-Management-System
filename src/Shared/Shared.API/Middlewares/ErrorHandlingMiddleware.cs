using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Shared.Domain.Exceptions;

namespace Shared.API.Middlewares;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public ErrorHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch(HAMSValidationException ex)
		{
			await HandleValidationExceptionAsync(context, ex);
		}
		catch (Exception ex)
		{
			Log.Error($"Unhandled exception caught: {ex.Message} {ex.Source} {ex.InnerException} {ex.StackTrace}");
			await HandleExceptionAsync(context, ex);
		}
	}

	private Task HandleValidationExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		context.Response.StatusCode = StatusCodes.Status400BadRequest;

		var response = new
		{
			exception.Message
		};

		return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		context.Response.StatusCode = StatusCodes.Status500InternalServerError;

		var response = new
		{
			Message = "An internal server error occurred.",
		};

		return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
	}
}
