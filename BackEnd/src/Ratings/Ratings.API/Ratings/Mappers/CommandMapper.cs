using Ratings.API.Ratings.Models.Requests;
using Ratings.API.Ratings.Models.Responses;
using Ratings.Application.Features.Ratings.Commands.AddRating;
using Ratings.Application.Features.Ratings.Commands.EditRating;
using Ratings.Application.Features.Ratings.Models;

namespace Ratings.API.Ratings.Mappers;

public static class CommandMapper
{
    public static AddRatingCommand ToCommand(
        this AddRatingRequest request,
        string userId)
        => new(
            userId,
            request.AppointmentId,
            request.Score,
            request.Comment);
    
    public static RatingCommandResponse ToResponse(
        this RatingCommandViewModel viewModel)
        => new(
            viewModel.Id);
    
    public static EditRatingCommand ToCommand(
        this EditRatingRequest request,
        string ratingId,
        string userId)
        => new(
            userId,
            ratingId,
            request.Score,
            request.Comment);
}
  