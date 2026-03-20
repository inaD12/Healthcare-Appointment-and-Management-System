using FluentValidation;
using Ratings.Domain.Entities;
using Ratings.Domain.Utilities;

namespace Ratings.Application.Features.Ratings.Queries.GetAllRatingsByDoctor;

public class GetAllRatingsByDoctorQueryValidator : AbstractValidator<GetAllRatingsByDoctorQuery>
{
	public GetAllRatingsByDoctorQueryValidator()
	{
		RuleFor(q => q.PatientId)
		   .MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
		   .MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.PatientId));

		RuleFor(q => q.DoctorId)
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH);
		
		RuleFor(q => q.AppointmentId)
			.MinimumLength(RatingsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(RatingsBusinessConfiguration.ID_MAX_LENGTH)
			.When(q => !string.IsNullOrEmpty(q.AppointmentId));
		
		RuleFor(x => x.MinScore)
			.InclusiveBetween(
				RatingsBusinessConfiguration.MIN_RATING_SCORE,
				RatingsBusinessConfiguration.MAX_RATING_SCORE)
			.When(q => q.MinScore.HasValue);
		
		RuleFor(x => x.MaxScore)
			.InclusiveBetween(
				RatingsBusinessConfiguration.MIN_RATING_SCORE,
				RatingsBusinessConfiguration.MAX_RATING_SCORE)
			.When(q => q.MaxScore.HasValue)
			.Must((query, maxScore) => !query.MinScore.HasValue || maxScore! >= query.MinScore)
			.WithMessage("MaxScore must be greater than or equal to MinScore.");

		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty).WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(Rating).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
