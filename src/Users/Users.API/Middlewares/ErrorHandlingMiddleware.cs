using Newtonsoft.Json;
using Serilog;

namespace Healthcare_Appointment_and_Management_System.Middlewares
{
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
			catch (Exception ex)
			{
				Log.Error($"Unhandled exception caught: {ex.Message} {ex.Source} {ex.InnerException} {ex.StackTrace}");
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
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
}
