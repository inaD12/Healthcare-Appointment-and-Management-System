using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using Contracts.Results;
using NSubstitute;

namespace Appointments.Application.UnitTests.Utilities;

public abstract class BaseAppointmentsUnitTest
{
	protected IRepositoryManager RepositoryMagager { get; }
	protected IFactoryManager FactoryMagager { get; }
	protected IJWTUserExtractor JWTUserExtractor { get; }
	protected IAppointmentCommandHandlerHelper AppointmentCommandHandlerHelper { get; }

	protected BaseAppointmentsUnitTest()
	{
		RepositoryMagager = Substitute.For<IRepositoryManager>();
		FactoryMagager = Substitute.For<IFactoryManager>();
		JWTUserExtractor = Substitute.For<IJWTUserExtractor>();
		AppointmentCommandHandlerHelper = Substitute.For<IAppointmentCommandHandlerHelper>();

		var appointment = new Appointment(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.SoonDate,
			AppointmentsTestUtilities.FutureDate,
			Domain.Enums.AppointmentStatus.Scheduled
			);

		string id = "";
		RepositoryMagager.Appointment.GetByIdAsync(Arg.Do<string>(a => id = a))
				.Returns(callInfo =>
				{
					if (id == AppointmentsTestUtilities.InvalidId)
						return (Result<Appointment>.Failure(Responses.AppointmentNotFound));

					return (Result<Appointment>.Success(appointment));
				});

		JWTUserExtractor.GetUserIdFromTokenAsync()
			.Returns(callInfo =>
			{
				 if (id == AppointmentsTestUtilities.IdForWrontIdFromToken)
					return Result<string>.Success(AppointmentsTestUtilities.InvalidId);
				if (id == AppointmentsTestUtilities.IdForJWTExtractorInternalError)
					return Result<string>.Failure(Responses.InternalError);

				return Result<string>.Success(AppointmentsTestUtilities.ValidId);
			 });

		RepositoryMagager.Appointment.ChangeStatusAsync(Arg.Any<Appointment>(), Domain.Enums.AppointmentStatus.Cancelled)
			.Returns(Result.Success());

		RepositoryMagager.Appointment.ChangeStatusAsync(Arg.Any<Appointment>(), Domain.Enums.AppointmentStatus.Cancelled)
			.Returns(callInfo =>
			{
				if (id == AppointmentsTestUtilities.IdForChangeStatusInternalError)
					return (Result.Failure(Responses.InternalError));

				return Result.Success();
			});
	}
}
