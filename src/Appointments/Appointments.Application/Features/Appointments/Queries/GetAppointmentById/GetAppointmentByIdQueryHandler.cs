using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Abstractions.Repository;
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
		var appointmentRes = await _appointmentRepository.GetByIdAsync(request.Id);
		if (appointmentRes.IsFailure)
			return Result<AppointmentQueryViewModel>.Failure(appointmentRes.Response);

		var appointment = appointmentRes.Value!;
		var appointmentQueryViewModel = _mapper.Map<AppointmentQueryViewModel>(appointment);

		return Result<AppointmentQueryViewModel>.Success(appointmentQueryViewModel);
	}
}
