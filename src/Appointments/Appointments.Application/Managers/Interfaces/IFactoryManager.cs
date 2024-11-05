﻿using Appointments.Application.Factories;

namespace Appointments.Application.Managers.Interfaces
{
    public interface IFactoryManager
    {
        IAppointmentFactory Appointment { get; }
    }
}