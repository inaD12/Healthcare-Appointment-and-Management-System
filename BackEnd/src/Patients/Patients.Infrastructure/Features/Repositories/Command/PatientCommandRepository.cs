using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Patients.Abstractions;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories.Command;

public class PatientCommandRepository(PatientsCommandDbContext context): GenericRepository<Patient>(context), IPatientCommandRepository
{
    public override async Task<Patient?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.Patients
            .Include("_conditions")
            .Include("_allergies")
            .FirstOrDefaultAsync(p => p.UserId == id, cancellationToken);
    }
}