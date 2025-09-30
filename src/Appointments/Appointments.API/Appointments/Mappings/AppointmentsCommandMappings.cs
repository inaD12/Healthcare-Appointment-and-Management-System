using Appointments.API.Appointments.Models.Requests;
using Appointments.API.Appointments.Models.Responses;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using AutoMapper;
using Shared.API.Models;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Shared.Utilities;

namespace Appointments.API.Appointments.Mappings;

public class AppointmentsCommandMappings : Profile
{
	public AppointmentsCommandMappings()
	{
		CreateMap<CreateAppointmentRequest, CreateAppointmentCommand>();

		CreateMap<(CancelAppointmentRequest, string id), CancelAppointmentCommand>()
			.ConstructUsing(src => new(
				src.Item1.AppointmentId,
				src.Item2));

		CreateMap<(RescheduleAppointmentRequest, string id, bool isAdmin), RescheduleAppointmentCommand>()
			.ConstructUsing(src => new(
				src.Item1.AppointmentID,
				src.Item2,
				src.Item1.ScheduledStartTime,
				src.Item1.Duration,
				src.Item3));

		CreateMap<AppointmentCommandViewModel, AppointmentCommandResponse>();
	}
}
