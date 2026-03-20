using Ratings.Application.Features.Ratings.Models;
using Ratings.Application.Features.Ratings.Queries.GetAllRatingsByDoctor;
using Ratings.Domain.Entities;
using Ratings.Domain.Models;
using Shared.Domain.Models;

namespace Ratings.Application.Features.Ratings.Mappers;

public static class QueryMapper
{
    public static RatingQueryViewModel ToQueryViewModel(
        this Rating rating)
        => new(
            rating.Id,
            rating.DoctorId,
            rating.PatientId,
            rating.AppointmentId,
            rating.Score,
            rating.CreatedAt,
            rating.Comment);
    
    public static DoctorRatingStatsQueryViewModel ToQueryViewModel(
        this DoctorRatingStats ratingStats)
        => new(
            ratingStats.Id,
            ratingStats.AverageRating,
            ratingStats.RatingsCount);
    
    public static RatingPagedListQuery ToInfraQuery(
        this GetAllRatingsByDoctorQuery query)
        => new(
            query.PatientId,
            query.DoctorId,
            query.AppointmentId,
            query.MinScore,
            query.MaxScore,
            query.SortOrder,
            query.SortPropertyName,
            query.Page,
            query.PageSize);
    
    public static RatingPaginatedQueryViewModel ToViewModel(
        this PagedList<Rating> pagedList)
        => new(
            pagedList.Items.Select(i => i.ToQueryViewModel()).ToList(),
            pagedList.Page,
            pagedList.PageSize,
            pagedList.TotalCount,
            pagedList.HasNextPage,
            pagedList.HasPreviousPage);
}