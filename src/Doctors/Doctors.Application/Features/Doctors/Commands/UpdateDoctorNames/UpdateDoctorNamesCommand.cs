using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorNames;

public sealed record UpdateDoctorNamesCommand(
    string UserId,
    string? FirstName = null,
    string? LastName = null) : ICommand;
