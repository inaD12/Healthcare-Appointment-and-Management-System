using Microsoft.EntityFrameworkCore;
using Patients.Application.Features.Encounters.Dtos;
using Patients.Infrastructure.Features.DBContexts;

namespace Patients.Application.Features.Encounters.Queries;

public sealed class EncounterQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncounterListItemDto> GetEncountersByPatient(
        string patientId,
        [Service] PatientsReadDbContext db)
    {
        return db.Encounters
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .Select(e => new EncounterListItemDto(
                e.Id,
                e.StartedAt,
                e.Status,
                e.DoctorId,
                e.PatientId));
    }

    public IQueryable<EncounterDetailsDto> GetEncounterDetails(
        string encounterId,
        [Service] PatientsReadDbContext db)
    {
        return db.Encounters
            .AsNoTracking()
            .Where(e => e.Id == encounterId)
            .Select(e => new EncounterDetailsDto(
                e.Id,
                e.StartedAt,
                e.FinalizedAt,
                e.Status,
                e.Notes.Select(n => new NoteDto(n.Id, n.Text, n.CreatedAt)).ToList(),
                e.Diagnoses.Select(d => new DiagnosisDto(d.Id, d.IcdCode, d.Description)).ToList(),
                e.Prescriptions.Select(p => new PrescriptionDto(p.Id, p.MedicationName, p.Dosage, p.Instructions)).ToList(),
                e.Addendums.Select(a => new AddendumDto(a.Id, a.Text, a.CreatedAt)).ToList()));
    }
}
