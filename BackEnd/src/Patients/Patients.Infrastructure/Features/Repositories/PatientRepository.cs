using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class PatientRepository(PatientsDbContext context) : GenericRepository<Patient>(context), IPatientRepository
{
    public override Task<Patient?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return context.Patients
            .Where(p => p.UserId == id).FirstOrDefaultAsync(cancellationToken);
    }

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

    public async Task<PatientHeaderDto> GetHeaderAsync(string userId)
    {
        var patient = await context.Patients
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => new
            {
                p.Id,
                FullName = p.FirstName + " " + p.LastName,
                p.BirthDate,
                p.Allergies,
                p.Conditions 
            })
            .FirstOrDefaultAsync();

        if (patient is null)
            return null!;

        var allergies = patient.Allergies.Select(a => a.Substance).ToList();
        var conditions = patient.Conditions.Select(c => c.Name).ToList();

        return new PatientHeaderDto(
            patient.Id,
            patient.FullName,
            patient.BirthDate,
            allergies,
            conditions
        );
    }
}