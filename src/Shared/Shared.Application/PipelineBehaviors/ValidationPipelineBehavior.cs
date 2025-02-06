using FluentValidation;
using MediatR;
using Serilog;

namespace Shared.Application.PipelineBehaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var validationContext = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(
			_validators.Select(v => v.ValidateAsync(validationContext)));

		var validationFailures = validationResults
			.Where(vr => !vr.IsValid)
			.SelectMany(vr => vr.Errors)
			.Select(vr => vr.ErrorMessage);

		if (validationFailures.Any())
		{
			var errorMessage = string.Join(",\n", validationFailures);
			Log.Error($"ValidationPipelineBehavior Error :{ errorMessage}");

			throw new ValidationException(errorMessage);
		}

		return await next();
	}
}