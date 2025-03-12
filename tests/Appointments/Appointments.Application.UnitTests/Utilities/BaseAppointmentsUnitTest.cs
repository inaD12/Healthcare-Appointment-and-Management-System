using Appointments.Application.Features.Appointments.Mappings;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Mappings;
using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using Appointments.Domain.ValueObjects;
using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.Helpers.Abstractions;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
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
		
		var AppointmentCommandViewModel = new AppointmentCommandViewModel(AppointmentsTestUtilities.ValidId);
		DateTimeRange = DateTimeRange.Create(AppointmentsTestUtilities.SoonDate, AppointmentsTestUtilities.FutureDate);

		var appointment = new Appointment(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentStatus.Scheduled,
			DateTimeRange
			)
		{
			Id = AppointmentsTestUtilities.ValidId
		};

		SceduledAppointmentList = new List<Appointment> { appointment };

		var doctorData = new UserData
		(
			AppointmentsTestUtilities.DoctorId,
			AppointmentsTestUtilities.DoctorEmail,
			Roles.Doctor
		);

		var patientData = new UserData
		(
			AppointmentsTestUtilities.PatientId,
			AppointmentsTestUtilities.PatientEmail,
			Roles.Patient
		);

		var appointmentWithDetailsDTO = new AppointmentWithDetailsModel
		{
			DoctorId = AppointmentsTestUtilities.ValidId,
			PatientId = AppointmentsTestUtilities.ValidId,
		};

		var appointmentWithDetailsDTONotMatching = new AppointmentWithDetailsModel();

		string id = "";
		AppointmentRepository.GetByIdAsync(Arg.Do<string>(a => id = a))
				.Returns(callInfo =>
				{
					if (id == AppointmentsTestUtilities.InvalidId)
						return (Result<Appointment>.Failure(ResponseList.AppointmentNotFound));

					return (Result<Appointment>.Success(appointment));
				});

		JWTParser.GetIdFromToken()
			.Returns(callInfo =>
			{
				if (id == AppointmentsTestUtilities.WrongIdFromTokenId)
					return Result<string>.Success(AppointmentsTestUtilities.InvalidId);
				if (id == AppointmentsTestUtilities.JWTExtractorInternalErrorId)
					return Result<string>.Failure(ResponseList.InternalError);

				return Result<string>.Success(AppointmentsTestUtilities.ValidId);
			});

		//RepositoryMagager.Appointment.ChangeStatusAsync(Arg.Any<Appointment>(), (Arg.Any<AppointmentStatus>()))
		//	.Returns(callInfo =>
		//	{
		//		if (id == AppointmentsTestUtilities.ChangeStatusInternalErrorId)
		//			return (Result.Failure(Responses.InternalError));

		//		return Result.Success();
		//	});

		AppointmentRepository.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
		   .Returns(Result<List<Appointment>>.Success(SceduledAppointmentList));


		UserDataRepository
			.GetUserDataByEmailAsync(Arg.Any<string>())
			.Returns(callInfo =>
			 {
				 string email = callInfo.ArgAt<string>(0);

				 if (email == AppointmentsTestUtilities.PatientEmail)
					 return Result<UserData>.Success(patientData);
				 if (email == AppointmentsTestUtilities.DoctorEmail)
					 return Result<UserData>.Success(doctorData);

				 return Result<UserData>.Failure(ResponseList.UserDataNotFound);
			 });


		AppointmentRepository
			.GetAppointmentWithUserDetailsAsync(Arg.Do<string>(a => id = a))
			.Returns(callInfo =>
			 {
				 string id = callInfo.ArgAt<string>(0);

				 if (id == AppointmentsTestUtilities.InvalidId)
					 return Result<AppointmentWithDetailsModel>.Failure(ResponseList.AppointmentNotFound);
				 if (id == AppointmentsTestUtilities.UnauthUserId)
					 return Result<AppointmentWithDetailsModel>.Success(appointmentWithDetailsDTONotMatching);

				 return Result<AppointmentWithDetailsModel>.Success(appointmentWithDetailsDTO);
			 });

		AppointmentRepository
			.IsTimeSlotAvailableAsync(Arg.Any<string>(), Arg.Any<DateTimeRange>())
			.Returns(callInfo =>
			{
				string id = callInfo.ArgAt<string>(0);

				if (id == AppointmentsTestUtilities.TimeSlotUnavailableId)
					return false;

				return true;
			});
	}


	protected void SetupGetAppointmentsToCompleteResult(Result<List<Appointment>> result)
	{
		AppointmentRepository.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(result);
	}
}
