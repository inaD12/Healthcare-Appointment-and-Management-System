using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;
using AutoMapper;

namespace Appointments.Application.Features.Appointments.Mappings;

public class AppointmentCommandProfile : Profile
{
	public AppointmentCommandProfile()
	{
		CreateMap<Appointment, AppointmentCommandViewModel>();
	}
}