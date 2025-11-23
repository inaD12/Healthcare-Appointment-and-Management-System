using System.Security.Claims;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using Appointments.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using NSubstitute;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Enums;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;
using ResponseList = Users.Domain.Utilities.ResponseList;

namespace Appointments.Application.UnitTests.Utilities;

public abstract class BaseAppointmentsUnitTest : BaseSharedUnitTest
{
	protected IDateTimeProvider DateTimeProvider { get; }
	protected IAppointmentRepository AppointmentRepository { get; }
	protected IRolesService RolesService { get; }
	protected IAuthorizationService AuthService { get; }

	protected BaseAppointmentsUnitTest()
		: base(
			Substitute.For<IUnitOfWork>()
		)
	{
		DateTimeProvider = Substitute.For<IDateTimeProvider>();
		RolesService = Substitute.For<IRolesService>();
		AppointmentRepository = Substitute.For<IAppointmentRepository>();
		AuthService = Substitute.For<IAuthorizationService>();

		DateTimeProvider.UtcNow.Returns(AppointmentsTestUtilities.CurrentDate);
		
		RolesService.GetUserRolesAsync(AppointmentsTestUtilities.InvalidId, Arg.Any<CancellationToken>())
			.Returns(Result<RolesResponse>.Failure(ResponseList.UserNotFound));
	}

	public List<Appointment> GetAppointmentList()
	{
		var appointment = GetAppointment();
		var scheduledAppointmentList = new List<Appointment> { appointment };

		AppointmentRepository.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(scheduledAppointmentList);

		return scheduledAppointmentList;
	}

	public Appointment GetAppointment(bool isCanceled = false)
	{
		var dateTimeRange = DateTimeRange.Create(AppointmentsTestUtilities.SoonDate, AppointmentsTestUtilities.FutureDate);

		var appointment = Appointment.Schedule(
			AppointmentsTestUtilities.PatientId,
			AppointmentsTestUtilities.DoctorId,
			dateTimeRange
		);
		if (isCanceled)
			appointment.Cancel(AppointmentsTestUtilities.PastDate);

		var scheduledAppointmentList = new List<Appointment> { appointment };

		var pagedList = new PagedList<Appointment>(
			scheduledAppointmentList,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue,
			scheduledAppointmentList.Count);

		var doctorRoles = new RolesResponse(
			new HashSet<string>([nameof(Roles.Doctor)])
		);

		var patientRoles = new RolesResponse(
			new HashSet<string>([nameof(Roles.Patient)])
		);

		AuthService
			.AuthorizeAsync(
				Arg.Any<ClaimsPrincipal>(), 
				Arg.Any<Appointment>(), 
				Arg.Any<IEnumerable<IAuthorizationRequirement>>())
			.Returns(Task.FromResult(AuthorizationResult.Success()));

		AppointmentRepository.IsTimeSlotAvailableAsync(appointment.DoctorId, Arg.Any<DateTimeRange>())
			.Returns(true);

		AppointmentRepository.GetByIdAsync(appointment.Id)
			.Returns(appointment);

		RolesService.GetUserRolesAsync(appointment.DoctorId, Arg.Any<CancellationToken>())
			.Returns(Result<RolesResponse>.Success(doctorRoles));
		
		RolesService.GetUserRolesAsync(appointment.PatientId, Arg.Any<CancellationToken>())
			.Returns(Result<RolesResponse>.Success(patientRoles));

		AppointmentRepository.GetAllAsync(Arg.Is<AppointmentPagedListQuery>(q => 
																				q.PatientId == appointment.PatientId &&
																				q.DoctorId == appointment.DoctorId))
			.Returns(pagedList);

		return appointment;
	}
}
