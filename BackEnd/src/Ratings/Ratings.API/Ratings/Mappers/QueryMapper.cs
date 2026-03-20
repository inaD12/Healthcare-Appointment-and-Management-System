using Ratings.API.Ratings.Models.Requests;
using Ratings.API.Ratings.Models.Responses;
using Ratings.Application.Features.Ratings.Models;
using Ratings.Application.Features.Ratings.Queries.GetAllRatingsByDoctor;

namespace Ratings.API.Ratings.Mappers;

public static class QueryMapper
{
    public static RatingQueryResponse ToResponse(
        this RatingQueryViewModel viewModel)
        => new(
            viewModel.Id,
            viewModel.DoctorId,
            viewModel.PatientId,
            viewModel.AppointmentId,
            viewModel.Score,
            viewModel.CreatedAt,
            viewModel.Comment);

    public static DoctorRatingStatsQueryResponse ToResponse(
        this DoctorRatingStatsQueryViewModel viewModel)
        => new(
            viewModel.DoctorId,
            viewModel.AvarageRating,
            viewModel.RatingCount);
    
    public static GetAllRatingsByDoctorQuery ToQuery(
        this GetAllRatingsByDoctorRequest request,
        string doctorId)
        => new(
            doctorId,
            request.PatientId,
            request.AppointmentId,
            request.MinScore,
            request.MaxScore,
            request.SortOrder,
            request.SortPropertyName,
            request.Page,
            request.PageSize);
    
    public static RatingPaginatedQueryResponse ToResponse(
        this RatingPaginatedQueryViewModel viewModel)
        => new(
            viewModel.Items,
            viewModel.Page,
            viewModel.PageSize,
            viewModel.TotalCount,
            viewModel.HasNextPage,
            viewModel.HasPreviousPage);
}