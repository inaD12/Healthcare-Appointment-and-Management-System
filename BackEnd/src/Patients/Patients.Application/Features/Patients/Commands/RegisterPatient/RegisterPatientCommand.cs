using Patients.Application.Features.Patients.Models;
using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.RegisterPatient;

public sealed record RegisterPatientCommand(
    string UserId,
    string FirstName,
    string LastName,
    DateOnly BirthDate) : ICommand<PatientCommandViewModel>;