using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Domain.Exceptions;

namespace Shared.API.ExceptionHandlers;

internal sealed class ValidationExceptionHandler(
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not HamsValidationException validationException)
            return false;

        Log.Warning(exception, "Validation exception occurred");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            }
        };

        var errors = validationException.Errors
            .GroupBy(e => e.Property)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
            );

        context.ProblemDetails.Extensions["errors"] = errors;

        return await problemDetailsService.TryWriteAsync(context);
    }
}