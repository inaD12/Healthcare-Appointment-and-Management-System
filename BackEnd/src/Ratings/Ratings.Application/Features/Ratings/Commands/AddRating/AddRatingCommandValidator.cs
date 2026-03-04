using FluentValidation;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Commands.AddRating;

public sealed class AddRatingCommandValidator 
	: AbstractValidator<AddRatingCommand>
{
	public AddRatingCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.AppointmentId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.Score)
			.InclusiveBetween(
				RatingsBusinessConfiguration.MIN_RATING_SCORE,
				RatingsBusinessConfiguration.MAX_RATING_SCORE)
			.WithMessage($"Score must be between {RatingsBusinessConfiguration.MIN_RATING_SCORE} and {RatingsBusinessConfiguration.MAX_RATING_SCORE}");

		RuleFor(x => x.Comment)
			.MaximumLength(RatingsBusinessConfiguration.MAX_COMMENT_LENGTH)
			.When(x => !string.IsNullOrWhiteSpace(x.Comment));
	}
}