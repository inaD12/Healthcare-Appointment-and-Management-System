using Patients.Application.Features.Patients.Models;
using Patients.Domain.Entities;

namespace Patients.Application.Features.Patients.Mappers;

public static class CommandMapper
{
    public static PatientCommandViewModel ToCommandViewModel(
        this Patient patient)
        => new(
            patient.Id);
}
  