using Appointments.API.Appointments.Mappers;
using Appointments.API.Appointments.Models.Requests;
using Appointments.API.Appointments.Models.Responses;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Infrastructure.Authentication;

namespace Appointments.API.Appointments.EndPoints;

internal class AppointmentsEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
	    var group = app.MapGroup("api/appointments");

	    group.MapPost("/", CreateAsync)
	        .Produces<AppointmentCommandResponse>(StatusCodes.Status201Created)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.CreateAppointment);

	    group.MapGet("/", GetAllAsync)
	        .Produces<AppointmentPaginatedQueryResponse>()
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.GetAppointment);

	    group.MapGet("/{id}", GetByIdAsync)
	        .Produces<AppointmentQueryResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.GetAppointment);
	    
	    group.MapGet("/by-doctor/{doctorUserId}", GetBookingsByDoctorAndDateAsync)
		    .Produces<ICollection<BookingQueryResponse>>()
		    .Produces(StatusCodes.Status400BadRequest)
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization(Permissions.GetAppointment);

	    group.MapDelete("/{id}", CancelAsync)
	        .Produces(StatusCodes.Status200OK)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.CancelAppointment);

	    group.MapPut("/{id}", RescheduleAsync)
	        .Produces<AppointmentCommandResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.RescheduleAppointment);
	}


	private async Task<IResult> CreateAsync(
		[FromBody] CreateAppointmentRequest request,
		HttpContext httpContext, 
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}

	private async Task<IResult> CancelAsync(
		[FromBody] CancelAppointmentRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> RescheduleAsync(
		[FromBody] RescheduleAppointmentRequest request,
		[FromServices] ISender sender,	
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}

	private async Task<IResult> GetAllAsync(
		[AsParameters] GetAllAppointmentsRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = request.ToQuery();
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}
	private async Task<IResult> GetByIdAsync(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = new GetAppointmentByIdQuery(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}
	
	private async Task<IResult> GetBookingsByDoctorAndDateAsync(
		[FromRoute] string doctorUserId,
		[AsParameters] GetBookingsByDoctorAndDateRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = request.ToQuery(doctorUserId);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var response = res.Value!.ToCollectionResponse();
		return ControllerResponse.ParseAndReturnMessage(res, response);
	}
}
