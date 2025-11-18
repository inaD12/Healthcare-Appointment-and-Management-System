using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;

public class GetAllAppointmentsQueryHandler : IQueryHandler<GetAllAppointmentsQuery, AppointmentPaginatedQueryViewModel>
{
	private readonly IAppointmentRepository _appointmentRepository;

	public GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
	{
		_appointmentRepository = appointmentRepository;
	}

	public async Task<Result<AppointmentPaginatedQueryViewModel>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
	{
		var appointmentPagedListQuery = request.ToPagedListQuery();
		var appointmentPagedList = await _appointmentRepository.GetAllAsync(appointmentPagedListQuery, cancellationToken);
		if (appointmentPagedList == null)
			return Result<AppointmentPaginatedQueryViewModel>.Failure(ResponseList.NoAppointmentsFound);

		var appointmentPaginatedQueryViewModel = appointmentPagedList.ToViewModel();
		return Result<AppointmentPaginatedQueryViewModel>.Success(appointmentPaginatedQueryViewModel);
	}
}
