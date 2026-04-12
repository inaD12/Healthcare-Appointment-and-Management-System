using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class PatientRepository(IDbContextFactory<PatientsDbContext> factory)
    : GenericFactoryRepository<PatientsDbContext, Patient>(factory),
        IPatientRepository
{
    public override async Task<Patient?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var db = CreateDbContext();

        return await db.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == id, cancellationToken);
    }
    public async Task<PatientHeaderDto> GetHeaderAsync(string userId)
    {
        await using var db = CreateDbContext();

        var patient = await db.Patients
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

        return new PatientHeaderDto(
            patient.Id,
            patient.FullName,
            patient.BirthDate,
            patient.Allergies.Select(a => a.Substance).ToList(),
            patient.Conditions.Select(c => c.Name).ToList()
        );
    }
}