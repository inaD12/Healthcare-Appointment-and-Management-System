﻿using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;

public class GetAllAppointmentsQueryHandler : IQueryHandler<GetAllAppointmentsQuery, AppointmentPaginatedQueryViewModel>
{
	private readonly IHAMSMapper _hamsMapper;
	private readonly IAppointmentRepository _appointmentRepository;

	public GetAllAppointmentsQueryHandler(IHAMSMapper hamsMapper, IAppointmentRepository appointmentRepository)
	{
		_hamsMapper = hamsMapper;
		_appointmentRepository = appointmentRepository;
	}

	public async Task<Result<AppointmentPaginatedQueryViewModel>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
	{
		var appointmentPagedListQuery = _hamsMapper.Map<AppointmentPagedListQuery>(request);
		var appointmentPagedList = await _appointmentRepository.GetAllAsync(appointmentPagedListQuery, cancellationToken);
		if (appointmentPagedList == null)
			return Result<AppointmentPaginatedQueryViewModel>.Failure(ResponseList.NoAppointmentsFound);

		var appointmentPaginatedQueryViewModel = _hamsMapper.Map<AppointmentPaginatedQueryViewModel>(appointmentPagedList);
		return Result<AppointmentPaginatedQueryViewModel>.Success(appointmentPaginatedQueryViewModel);
	}
}
