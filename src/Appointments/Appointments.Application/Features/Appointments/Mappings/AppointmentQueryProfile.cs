using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;
using AutoMapper;

namespace Appointments.Application.Features.Appointments.Mappings;

public class AppointmentQueryProfile : Profile
{
	public AppointmentQueryProfile()
	{
		CreateMap<Appointment, AppointmentQueryViewModel>();
	}
}
