﻿using Appointments.Infrastructure.Repositories;

namespace Appointments.Application.Managers.Interfaces
{
    public interface IRepositoryManager
    {
        IAppointmentRepository Appointment { get; }
		IUserDataRepository UserData { get; }

	}
}