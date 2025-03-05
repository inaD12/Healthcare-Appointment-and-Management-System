using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using AutoMapper;

namespace Appointments.API.Appointments.Mappings;

public class AppointmentsQueryMappings : Profile
{
	public AppointmentsQueryMappings()
	{
		CreateMap<string, GetAppointmentByIdQuery>()
			.ConstructUsing(src => new(src));
	}
}
