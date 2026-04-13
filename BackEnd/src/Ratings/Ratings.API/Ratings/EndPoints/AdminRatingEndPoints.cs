using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ratings.API.Ratings.Mappers;
using Ratings.API.Ratings.Models.Requests;
using Ratings.API.Ratings.Models.Responses;
using Ratings.Application.Features.Ratings.Commands.RemoveRating;
using Ratings.Application.Features.Ratings.Queries.GetDoctorRatingStatsById;
using Ratings.Application.Features.Ratings.Queries.GetRatingByAppointmentId;
using Ratings.Application.Features.Ratings.Queries.GetRatingById;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Infrastructure.Authentication;

namespace Ratings.API.Ratings.EndPoints;

internal class AdminRatingEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
	    var adminGroup = app.MapGroup("api/admin/ratings");

	    adminGroup.MapDelete("/{id}", RemoveRatingByAdminAsync)
		    .Produces(StatusCodes.Status200OK)
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization(Permissions.RemoveRatingAdmin);
	}
	
	private async Task<IResult> RemoveRatingByAdminAsync(
		[FromRoute] string id,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new RemoveRatingCommand(userId, id, true);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}