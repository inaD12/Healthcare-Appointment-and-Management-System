using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ratings.API.Ratings.Mappers;
using Ratings.API.Ratings.Models.Requests;
using Ratings.API.Ratings.Models.Responses;
using Ratings.Application.Features.Ratings.Commands.RemoveRating;
using Ratings.Application.Features.Ratings.Queries.GetDoctorRatingStatsById;
using Ratings.Application.Features.Ratings.Queries.GetRatingById;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Infrastructure.Authentication;

namespace Ratings.API.Ratings.EndPoints;

internal class RatingEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
	    var ratingsGroup = app.MapGroup("api/ratings");

	    ratingsGroup.MapPost("/", AddRatingAsync)
	        .Produces<RatingCommandResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.AddRating);

	    ratingsGroup.MapPut("/{id}", EditRatingAsync)
		    .Produces(StatusCodes.Status200OK)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.EditRating);

	    ratingsGroup.MapDelete("/{id}", RemoveRatingAsync)
		    .Produces(StatusCodes.Status200OK)
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization(Permissions.RemoveRating);

	    ratingsGroup.MapGet("/{id}", GetRatingByIdAsync)
		    .Produces<RatingQueryResponse>()
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization(Permissions.GetRating);
	    
	    ratingsGroup.MapGet("/by-doctor/{doctorId}", GetAllByDoctorAsync)
		    .Produces<RatingPaginatedQueryResponse>()
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization(Permissions.GetRating);

	    var ratingStatsGroup = app.MapGroup("/api/ratingsStats");
	    
	    ratingStatsGroup.MapGet("/{id}", GetDoctorRatingStatsByIdAsync)
		    .Produces<DoctorRatingStatsQueryResponse>()
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization(Permissions.GetRatingStats);
	}

	private async Task<IResult> AddRatingAsync(
		[FromBody] AddRatingRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}
	
	private async Task<IResult> EditRatingAsync(
		[FromRoute] string id,
		[FromBody] EditRatingRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(id, userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> RemoveRatingAsync(
		[FromRoute] string id,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new RemoveRatingCommand(userId, id);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> GetDoctorRatingStatsByIdAsync(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = new GetDoctorRatingStatsByIdQuery(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var queryResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, queryResponse);
	}
	
	private async Task<IResult> GetRatingByIdAsync(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = new GetRatingByIdQuery(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var queryResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, queryResponse);
	}
	
	private async Task<IResult> GetAllByDoctorAsync(
		[FromRoute] string doctorId,
		[AsParameters] GetAllRatingsByDoctorRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = request.ToQuery(doctorId);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var response = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, response);
	}
}