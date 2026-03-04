using Ratings.API.Ratings.Models.Responses;
using Ratings.Application.Features.Ratings.Models;

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
            viewModel.Comment);

    public static DoctorRatingStatsQueryResponse ToResponse(
        this DoctorRatingStatsQueryViewModel viewModel)
        => new(
            viewModel.DoctorId,
            viewModel.AvarageRating,
            viewModel.RatingCount);
}