using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.AddAllergy;

public sealed record AddAllergyCommand(
    string Id,
    string Substance,
    string Reaction) : ICommand;