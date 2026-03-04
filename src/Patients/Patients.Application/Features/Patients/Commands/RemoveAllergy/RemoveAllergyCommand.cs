using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.RemoveAllergy;

public sealed record RemoveAllergyCommand(
    string Id,
    string AllergyId) : ICommand;