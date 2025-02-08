using Appointments.API.Extentions;
using Appointments.Application.Appoints.Commands.CancelAppointment;
using Appointments.Application.Appoints.Commands.CreateAppointment;
using Appointments.Application.Appoints.Commands.RescheduleAppointment;
using Appointments.Domain.DTOS.Request;
using MediatR;
using Shared.Domain.Results;

namespace Appointments.API.EndPoints;

internal class AppointmentsEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/appointments");

		group.MapPost("create", Create)
			.Produces<MessageDTO>(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces<MessageDTO>(StatusCodes.Status404NotFound)
			.Produces<MessageDTO>(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapPut("cancel-appointment", CancelAppointment)
			.Produces<MessageDTO>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces<MessageDTO>(StatusCodes.Status404NotFound)
			.Produces<MessageDTO>(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapPut("reschedule-appointment", RescheduleAppointment)
			.Produces<MessageDTO>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces<MessageDTO>(StatusCodes.Status404NotFound)
			.Produces<MessageDTO>(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();
	}

	public async Task<IResult> Create(
		CreateAppointmentDTO appointmentDTO,
		ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new CreateAppointmentCommand(
			appointmentDTO.PatientEmail,
			appointmentDTO.DoctorEmail,
			appointmentDTO.ScheduledStartTime,
			appointmentDTO.Duration);

		var res = await sender.Send(command, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> CancelAppointment(
		string appointmentId,
		ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new CancelAppointmentCommand(appointmentId);

		var res = await sender.Send(command, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> RescheduleAppointment(
		RescheduleAppointmentDTO appointmentDTO,
		ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new RescheduleAppointmentCommand(appointmentDTO.AppointmentID, appointmentDTO.ScheduledStartTime, appointmentDTO.Duration);

		var res = await sender.Send(command, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}
}
