using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;
using Appointments.Domain.Entities;
using Appointments.Domain.Infrastructure.Models;
using AutoMapper;
using Shared.Domain.Models;

namespace Appointments.Application.Features.Appointments.Mappings;

public class AppointmentQueryProfile : Profile
{
	public AppointmentQueryProfile()
	{
		CreateMap<Appointment, AppointmentQueryViewModel>();

		CreateMap<GetAllAppointmentsQuery, AppointmentPagedListQuery>();

		CreateMap<PagedList<Appointment>, AppointmentPaginatedQueryViewModel>();
	}
}
