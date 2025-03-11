using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using AutoMapper;

namespace Appointments.Application.Features.Appointments.Mappings;

public class AppointmentCommandProfile : Profile
{
	public AppointmentCommandProfile()
	{
		CreateMap<Appointment, AppointmentCommandViewModel>();
	}
}