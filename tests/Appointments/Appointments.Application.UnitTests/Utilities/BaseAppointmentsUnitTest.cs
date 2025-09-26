using Appointments.Application.Features.Appointments.Mappings;
using Appointments.Application.Features.Mappings;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Utilities;
using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Shared.Domain.Models;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.UnitTests.Utilities;

public abstract class BaseAppointmentsUnitTest : BaseSharedUnitTest
{
	protected IDateTimeProvider DateTimeProvider { get; }
	protected IAppointmentRepository AppointmentRepository { get; }
	protected IUserDataRepository UserDataRepository { get; }

	protected BaseAppointmentsUnitTest()
		: base(
			new HAMSMapper(
				new Mapper(
					new MapperConfiguration(cfg =>
					{
						cfg.AddProfile<AppointmentCommandProfile>();
						cfg.AddProfile<AppointmentQueryProfile>();
						cfg.AddProfile<AppointmentProfile>();
					})
				)
			),
			Substitute.For<IUnitOfWork>()
		)
	{
		DateTimeProvider = Substitute.For<IDateTimeProvider>();
		UserDataRepository = Substitute.For<IUserDataRepository>();
		AppointmentRepository = Substitute.For<IAppointmentRepository>();

		DateTimeProvider.UtcNow.Returns(AppointmentsTestUtilities.CurrentDate);
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

		var appointmentWithDetailsDto = new AppointmentWithDetailsModel
		{
			DoctorId = appointment.DoctorId,
			PatientId = appointment.PatientId,
			Appointment = appointment
		};

		var scheduledAppointmentList = new List<Appointment> { appointment };

		var pagedList = new PagedList<Appointment>(
			scheduledAppointmentList,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue,
			scheduledAppointmentList.Count);

		var doctorData = new UserData(
			AppointmentsTestUtilities.DoctorId,
			AppointmentsTestUtilities.DoctorEmail,
			[Role.Doctor]
		);

		var patientData = new UserData(
			AppointmentsTestUtilities.PatientId,
			AppointmentsTestUtilities.PatientEmail,
			[Role.Patient]
		);

		UserDataRepository.GetUserDataByEmailAsync(Arg.Any<string>()).Returns(callInfo =>
		{
			string email = callInfo.ArgAt<string>(0);

			if (email == AppointmentsTestUtilities.PatientEmail)
				return patientData;
			if (email == AppointmentsTestUtilities.DoctorEmail)
				return doctorData;

			return null;
		});

		AppointmentRepository.IsTimeSlotAvailableAsync(appointment.DoctorId, Arg.Any<DateTimeRange>())
			.Returns(true);

		AppointmentRepository.GetByIdAsync(appointment.Id)
			.Returns(appointment);

		AppointmentRepository.GetAppointmentWithUserDetailsAsync(appointment.Id)
			.Returns(appointmentWithDetailsDto);

		AppointmentRepository.GetAllAsync(Arg.Is<AppointmentPagedListQuery>(q => 
																				q.PatientId == appointment.PatientId &&
																				q.DoctorId == appointment.DoctorId))
			.Returns(pagedList);

		return appointment;
	}
}
