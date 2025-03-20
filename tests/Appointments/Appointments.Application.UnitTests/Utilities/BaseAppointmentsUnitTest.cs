using Appointments.Application.Features.Appointments.Mappings;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Mappings;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Utilities;
using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.Helpers.Abstractions;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.UnitTests.Utilities;

public abstract class BaseAppointmentsUnitTest : BaseSharedUnitTest
{
	protected IDateTimeProvider DateTimeProvider { get; }
	protected IJwtParser JWTParser { get; }
	protected IAppointmentRepository AppointmentRepository { get; }
	protected IUserDataRepository UserDataRepository { get; }

	protected readonly List<Appointment> SceduledAppointmentList;
	protected readonly DateTimeRange DateTimeRange;
	protected readonly AppointmentCommandViewModel AppointmentCommandViewModel;
	protected readonly Appointment Appointment;
	protected readonly Appointment AppointmentInvalid;
	protected readonly Appointment AppointmentCanceled;
	protected readonly UserData DoctorData;
	protected readonly UserData PatientData;
	protected readonly AppointmentWithDetailsModel AppointmentWithDetailsDTO;
	protected readonly AppointmentWithDetailsModel AppointmentWithDetailsDTONotMatching;
	protected readonly AppointmentWithDetailsModel AppointmentWithDetailsDTOCanceled;

	protected BaseAppointmentsUnitTest() : base(
		new HAMSMapper(
			new Mapper(
				new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<AppointmentCommandProfile>();
					cfg.AddProfile<AppointmentProfile>();
				}))),
		Substitute.For<IUnitOfWork>()
		)
	{
		DateTimeProvider = Substitute.For<IDateTimeProvider>();
		UserDataRepository = Substitute.For<IUserDataRepository>();
		AppointmentRepository = Substitute.For<IAppointmentRepository>();
		JWTParser = Substitute.For<IJwtParser>();
		
		AppointmentCommandViewModel = new AppointmentCommandViewModel(AppointmentsTestUtilities.ValidId);

		DateTimeRange = DateTimeRange.Create(AppointmentsTestUtilities.SoonDate, AppointmentsTestUtilities.FutureDate);

		Appointment = Appointment.Schedule(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			DateTimeRange
			);

		AppointmentInvalid = Appointment.Schedule(
			AppointmentsTestUtilities.InvalidId,
			AppointmentsTestUtilities.InvalidId,
			DateTimeRange
			);

		AppointmentCanceled = Appointment.Schedule(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			DateTimeRange
			);
		AppointmentCanceled.Cancel(AppointmentsTestUtilities.PastDate);

		SceduledAppointmentList = new List<Appointment> { Appointment };

		DoctorData = new UserData
		(
			AppointmentsTestUtilities.DoctorId,
			AppointmentsTestUtilities.DoctorEmail,
			Roles.Doctor
		);

		PatientData = new UserData
		(
			AppointmentsTestUtilities.PatientId,
			AppointmentsTestUtilities.PatientEmail,
			Roles.Patient
		);

		AppointmentWithDetailsDTO = new AppointmentWithDetailsModel
		{
			DoctorId = AppointmentsTestUtilities.DoctorId,
			PatientId = AppointmentsTestUtilities.PatientId,
			Appointment = Appointment
		};

		AppointmentWithDetailsDTONotMatching = new AppointmentWithDetailsModel();

		AppointmentWithDetailsDTOCanceled = new AppointmentWithDetailsModel
		{
			DoctorId = AppointmentsTestUtilities.DoctorId,
			PatientId = AppointmentsTestUtilities.PatientId,
			Appointment = AppointmentCanceled
		};

		AppointmentRepository.GetByIdAsync(AppointmentsTestUtilities.ValidId).Returns(Appointment);
		AppointmentRepository.GetByIdAsync(AppointmentsTestUtilities.WrongIdFromTokenId).Returns(AppointmentInvalid);

		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.ValidId);

		AppointmentRepository.GetAppointmentsToCompleteAsync(AppointmentsTestUtilities.CurrentDate).Returns(SceduledAppointmentList);

		UserDataRepository.GetUserDataByEmailAsync(Arg.Any<string>()).Returns(callInfo =>
			 {
				 string email = callInfo.ArgAt<string>(0);

				 if (email == AppointmentsTestUtilities.PatientEmail)
					 return PatientData;
				 if (email == AppointmentsTestUtilities.DoctorEmail)
					 return DoctorData;

				 return null;
			 });

		AppointmentRepository
			.GetAppointmentWithUserDetailsAsync(AppointmentsTestUtilities.ValidId).Returns(AppointmentWithDetailsDTO);

		AppointmentRepository
			.IsTimeSlotAvailableAsync(Arg.Any<string>(), Arg.Any<DateTimeRange>()).Returns(true);

		DateTimeProvider.UtcNow.Returns(AppointmentsTestUtilities.CurrentDate);
	}
}
