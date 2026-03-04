using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.AddSpeciality;

public sealed record AddSpecialityCommand(
    string UserId,
    string Speciality) : ICommand;
