using Appointments.API.Appointments.Models.Requests;
using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using AutoMapper;

namespace Appointments.API.Appointments.Mappings;

public class AppointmentsCommandMappings : Profile
{
	public AppointmentsCommandMappings()
	{
		CreateMap<CreateAppointmentRequest, CreateAppointmentCommand>();

		CreateMap<CancelAppointmentRequest, CancelAppointmentCommand>();

		CreateMap<RescheduleAppointmentRequest, RescheduleAppointmentCommand>();
	}
}
