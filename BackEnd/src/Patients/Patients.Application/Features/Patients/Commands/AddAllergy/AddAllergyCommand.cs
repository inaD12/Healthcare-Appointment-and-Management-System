using Patients.Application.Features.Patients.Models;
using Shared.Application.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.AddAllergy;

public sealed record AddAllergyCommand(
    string Id,
    string Substance,
    string Reaction) : ICommand<AllergyCommandViewModel>;