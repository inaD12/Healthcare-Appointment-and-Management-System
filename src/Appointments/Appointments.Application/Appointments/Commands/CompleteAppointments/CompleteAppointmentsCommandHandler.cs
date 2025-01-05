﻿using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Enums;
using Contracts.Abstractions.Messaging;
using Contracts.Results;
using Serilog;

namespace Appointments.Application.Appointments.Commands.CompleteAppointments
{
	internal sealed class CompleteAppointmentsCommandHandler : ICommandHandler<CompleteAppointmentsCommand>
	{
		private readonly IRepositoryManager _repositoryManager;

		public CompleteAppointmentsCommandHandler(IRepositoryManager repositoryManager)
		{
			_repositoryManager = repositoryManager;
		}

		public async Task<Result> Handle(CompleteAppointmentsCommand request, CancellationToken cancellationToken)
		{
			var now = DateTime.UtcNow;

			var appointmentsToCompleteRes = await _repositoryManager.Appointment
				.GetAppointmentsToCompleteAsync(now);
			if (appointmentsToCompleteRes.IsFailure)
			{
				return Result.Failure(appointmentsToCompleteRes.Response);
			}

			var appointmentsToComplete = appointmentsToCompleteRes.Value;

			foreach (var appointment in appointmentsToComplete)
			{
				appointment.Status = AppointmentStatus.Completed;
			}

			await _repositoryManager.Appointment.SaveChangesAsync();

			return Result.Success();
		}
	}
}