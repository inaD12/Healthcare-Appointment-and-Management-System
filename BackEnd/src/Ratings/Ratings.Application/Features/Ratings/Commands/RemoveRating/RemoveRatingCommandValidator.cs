using FluentValidation;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Commands.RemoveRating;

public sealed class RemoveRatingCommandValidator 
	: AbstractValidator<RemoveRatingCommand>
{
	public RemoveRatingCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.RatingId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
	}
}