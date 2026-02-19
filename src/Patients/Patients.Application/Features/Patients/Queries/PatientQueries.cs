using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Patients.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Patients.Queries;

public sealed class PatientQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PatientListItemDto> GetPatients([Service] PatientsReadDbContext db)
    {
        return db.Patients
            .AsNoTracking()
            .Select(p => new PatientListItemDto(
                p.Id,
                p.FirstName + " " + p.LastName,
                p.BirthDate));
    }

    public IQueryable<PatientHeaderDto> GetPatientHeader(
        string patientId,
        [Service] PatientsReadDbContext db)
    {
        return db.Patients
            .AsNoTracking()
            .Where(p => p.Id == patientId)
            .Select(p => new PatientHeaderDto(
                p.Id,
                p.FirstName + " " + p.LastName,
                p.BirthDate,
                p.Allergies.Select(a => a.Substance).ToList(),
                p.Conditions.Select(c => c.Name).ToList()));
    }
}
