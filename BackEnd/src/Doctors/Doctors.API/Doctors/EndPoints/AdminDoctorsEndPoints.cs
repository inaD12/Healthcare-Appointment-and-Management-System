using Doctors.API.Doctors.Mappers;
using Doctors.API.Doctors.Models.Requests;
using Doctors.Application.Features.Doctors.Commands.UpdateDoctorInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;

namespace Doctors.API.Doctors.EndPoints;

public class AdminDoctorsEndPoints  : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var adminGroup = app.MapGroup("/api/admin/doctors");
		
		adminGroup.MapPut("/user/{userId}", UpdateDoctorInfoAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.UpdateDoctorByAdmin);
		
		adminGroup.MapPost("/user/{userId}/specialities", AddSpecialityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.AddSpecialityByAdmin);
		
		adminGroup.MapDelete("/user/{userId}/specialities", DeleteSpecialityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.RemoveSpecialityByAdmin);
	}

	private async Task<IResult> UpdateDoctorInfoAsync(
		[FromBody] UpdateDoctorInfoByAdminRequest request,
		[FromRoute] string userId,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new UpdateDoctorInfoCommand(userId, request.NewBio);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> AddSpecialityAsync(
		[FromBody] AddSpecialityRequest request,
		[FromRoute] string userId,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> DeleteSpecialityAsync(
		[FromBody] RemoveSpecialityRequest request,
		[FromRoute] string userId,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}
