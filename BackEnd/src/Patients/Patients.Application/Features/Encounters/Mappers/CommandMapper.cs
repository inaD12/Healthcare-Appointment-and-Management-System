using Patients.Application.Features.Encounters.Models;
using Patients.Domain.Entities;

namespace Patients.Application.Features.Encounters.Mappers;

public static class CommandMapper
{
    public static EncounterCommandViewModel ToCommandViewModel(
        this Encounter encounter)
        => new(
            encounter.Id);
}
  