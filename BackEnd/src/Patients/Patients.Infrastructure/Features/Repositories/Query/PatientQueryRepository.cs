using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories.Query;

public class PatientQueryRepository(IDbContextFactory<PatientsQueryDbContext> factory)
    : GenericFactoryRepository<PatientsQueryDbContext, Patient>(factory),
        IPatientQueryRepository
{
    public override async Task<Patient?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var db = CreateDbContext();

        return await db.Patients
            .AsNoTracking()
            .Include("_conditions")
            .Include("_allergies")
            .FirstOrDefaultAsync(p => p.UserId == id, cancellationToken);
    }
    public async Task<PatientHeaderDto> GetHeaderAsync(string userId, CancellationToken cancellationToken = default)
    {
        await using var db = CreateDbContext();

        var patient = await GetByIdAsync(userId, cancellationToken);

        if (patient is null)
            return null!;
    
        return new PatientHeaderDto(
            patient.Id,
            patient.FirstName + " " + patient.LastName,
            patient.BirthDate,
            patient.Allergies.Select(a => new AllergyDto(
                a.Id,
                a.Substance,
                a.Reaction
            )),
            patient.Conditions.Select(c => new ConditionDto(
                c.Id,
                c.Name
            ))
        );
    }
}