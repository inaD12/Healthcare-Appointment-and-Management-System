using FluentValidation;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Commands.EditRating;

public sealed class EditRatingCommandValidator 
	: AbstractValidator<EditRatingCommand>
{
	public EditRatingCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.RatingId)
			.NotEmpty()
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(x => x.Score)
			.InclusiveBetween(
				RatingsBusinessConfiguration.MIN_RATING_SCORE,
				RatingsBusinessConfiguration.MAX_RATING_SCORE)
			.WithMessage($"Score must be between {RatingsBusinessConfiguration.MIN_RATING_SCORE} and {RatingsBusinessConfiguration.MAX_RATING_SCORE}")
			.When(x => x.Score != null);

		RuleFor(x => x.Comment)
			.MaximumLength(RatingsBusinessConfiguration.MAX_COMMENT_LENGTH)
			.When(x => !string.IsNullOrWhiteSpace(x.Comment));
		
		RuleFor(x => x)
			.Must(x => x.Score != null || x.Comment != null)
			.WithMessage("At least one field must be changed.");
	}
}