using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class EncounterRepository(PatientsDbContext context) : GenericRepository<Encounter>(context), IEncounterRepository
{
    public async Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default)
    {
        var res = await context.Encounters.SingleOrDefaultAsync(e => e.AppointmentId == appointmentId, cancellationToken);

        return res;
    }
    
    public IQueryable<Encounter> GetByPatient(string patientId)
    {
        var res = context.Encounters
            .AsNoTracking()
            .Where(e => e.PatientId == patientId);

        return res;
    }

    public async Task<List<EncounterListItemDto>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken)
    {
        return await context.Encounters
            .AsNoTracking()
            .Where(e => patientIds.Contains(e.PatientId))
            .Select(e => new EncounterListItemDto(
                e.Id,
                e.StartedAt,
                e.Status,
                e.DoctorId,
                e.PatientId))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<EncounterDetailsDto>> GetDetailsByAppointmentIdsAsync(
        IReadOnlyList<string> appointmentIds,
        CancellationToken cancellationToken)
    {
        var encounters = await context.Encounters
            .Where(e => appointmentIds.Contains(e.AppointmentId))
            .ToListAsync(cancellationToken);

        var result = encounters
            .OfType<Encounter>()
            .Select(e => new EncounterDetailsDto(
                e.Id,
                e.AppointmentId,
                e.DoctorId,
                e.PatientId,
                e.StartedAt,
                e.FinalizedAt,
                e.Status,
                e.Notes.Select(n => new NoteDto(n.Id, e.Id, n.Text, n.CreatedAt)).ToList(),
                e.Diagnoses.Select(d => new DiagnosisDto(d.Id, e.Id, d.IcdCode, d.Description)).ToList(),
                e.Prescriptions.Select(p => new PrescriptionDto(p.Id, e.Id, p.MedicationName, p.Dosage, p.Instructions)).ToList(),
                e.Addendums.Select(a => new AddendumDto(a.Id, e.Id, a.Text, a.CreatedAt)).ToList()
            ))
            .ToList();

        return result;
    }
    public async Task<List<AddendumDto>> GetAddendumsByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        return await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Addendums.Select(a => new AddendumDto(
                a.Id,
                e.Id,
                a.Text,
                a.CreatedAt)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DiagnosisDto>> GetDiagnosesByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        return await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Diagnoses.Select(d => new DiagnosisDto(
                d.Id,
                e.Id,
                d.IcdCode,
                d.Description)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<NoteDto>> GetNotesByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        return await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Notes.Select(n => new NoteDto(
                n.Id,
                e.Id,
                n.Text,
                n.CreatedAt)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PrescriptionDto>> GetPrescriptionsByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        return await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Prescriptions.Select(p => new PrescriptionDto(
                p.Id,
                e.Id,
                p.MedicationName,
                p.Dosage,
                p.Instructions)))
            .ToListAsync(cancellationToken);
    }
}