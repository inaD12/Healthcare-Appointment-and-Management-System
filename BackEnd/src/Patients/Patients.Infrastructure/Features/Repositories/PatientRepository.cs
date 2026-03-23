using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class PatientRepository(PatientsDbContext context) : GenericRepository<Patient>(context), IPatientRepository
{
    public IQueryable<PatientListItemDto> GetAll()
    {
        return context.Patients
            .AsNoTracking()
            .Select(p => new PatientListItemDto(
                p.Id,
                p.FirstName + " " + p.LastName,
                p.BirthDate
            ));
    }

    public IQueryable<PatientHeaderDto> GetHeader(string patientId)
    {
        return context.Patients
            .AsNoTracking()
            .Where(p => p.Id == patientId)
            .Select(p => new PatientHeaderDto(
                p.Id,
                p.FirstName + " " + p.LastName,
                p.BirthDate,
                p.Allergies.Select(a => a.Substance).ToList(),
                p.Conditions.Select(c => c.Name).ToList()
            ));
    }
}