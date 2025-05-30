using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointmentById;

public sealed class GetAppointmentByIdQueryHandler : IQueryHandler<GetAppointmentByIdQuery, AppointmentQueryViewModel>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IHAMSMapper _mapper;

	public GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository, IHAMSMapper mapper)
	{
		_appointmentRepository = appointmentRepository;
		_mapper = mapper;
	}

	public async Task<Result<AppointmentQueryViewModel>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
	{
		var appointment = await _appointmentRepository.GetByIdAsync(request.Id);
		if (appointment == null)
			return Result<AppointmentQueryViewModel>.Failure(ResponseList.AppointmentNotFound);

		var appointmentQueryViewModel = _mapper.Map<AppointmentQueryViewModel>(appointment);
		return Result<AppointmentQueryViewModel>.Success(appointmentQueryViewModel);
	}
}
