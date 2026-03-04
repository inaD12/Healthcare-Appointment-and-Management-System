using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.DeletePatient;

public sealed record DeletePatientCommand(
    string Id) : ICommand;