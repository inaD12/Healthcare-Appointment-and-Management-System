using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RemoveSpeciality;

public sealed record RemoveSpecialityCommand(
    string UserId,
    string Speciality) : ICommand;
