using Appointments.API.Appointments.Models.Requests;
using Appointments.API.Appointments.Models.Responses;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;
using AutoMapper;

namespace Appointments.API.Appointments.Mappings;

public class AppointmentsQueryMappings : Profile
{
	public AppointmentsQueryMappings()
	{
		CreateMap<string, GetAppointmentByIdQuery>()
			.ConstructUsing(src => new(src));

		CreateMap<AppointmentQueryViewModel, AppointmentQueryResponse>();

		CreateMap<GetAllAppointmentsRequest, GetAllAppointmentsQuery>();

		CreateMap<AppointmentPaginatedQueryViewModel, AppointmentPaginatedQueryResponse>();
	}
}
