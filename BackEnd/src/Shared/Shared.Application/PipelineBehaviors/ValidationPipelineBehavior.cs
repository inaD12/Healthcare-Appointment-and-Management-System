using FluentValidation;
using MediatR;
using Shared.Domain.Exceptions;

namespace Shared.Application.PipelineBehaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
	IEnumerable<IValidator<TRequest>> validators)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		if (!validators.Any())
			return await next(cancellationToken);

		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(
			validators.Select(v => v.ValidateAsync(context, cancellationToken)));

		var failures = validationResults
			.Where(r => !r.IsValid)
			.SelectMany(r => r.Errors)
			.ToArray();

		if (failures.Length != 0)
			throw new HamsValidationException(failures);

		return await next(cancellationToken);
	}
}