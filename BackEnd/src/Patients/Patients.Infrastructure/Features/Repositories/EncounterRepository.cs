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
    
    public IQueryable<EncounterListItemDto> GetByPatient(string patientId)
    {
        return context.Encounters
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .Select(e => new EncounterListItemDto(
                e.Id,
                e.StartedAt,
                e.Status,
                e.DoctorId,
                e.PatientId));
    }

    public IQueryable<EncounterDetailsDto> GetDetails(string encounterId)
    {
        return context.Encounters
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
                e.StartedAt,
                e.FinalizedAt,
                e.Status,
                e.Notes.Select(n => new NoteDto(n.Id, n.Text, n.CreatedAt)).ToList(),
                e.Diagnoses.Select(d => new DiagnosisDto(d.Id, d.IcdCode, d.Description)).ToList(),
                e.Prescriptions.Select(p => new PrescriptionDto(p.Id, p.MedicationName, p.Dosage, p.Instructions)).ToList(),
                e.Addendums.Select(a => new AddendumDto(a.Id, a.Text, a.CreatedAt)).ToList()
            ))
            .ToList();

        return result;
    }
    public async Task<Dictionary<string, List<AddendumDto>>> GetAddendumsByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        var data = await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Addendums.Select(a => new
            {
                EncounterId = e.Id,
                Addendum = new AddendumDto(a.Id, a.Text, a.CreatedAt)
            }))
            .ToListAsync(cancellationToken);

        return data
            .GroupBy(x => x.EncounterId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Addendum).ToList());
    }

    public async Task<Dictionary<string, List<DiagnosisDto>>> GetDiagnosesByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        var data = await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Diagnoses.Select(d => new
            {
                EncounterId = e.Id,
                Diagnosis = new DiagnosisDto(d.Id, d.IcdCode, d.Description)
            }))
            .ToListAsync(cancellationToken);

        return data
            .GroupBy(x => x.EncounterId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Diagnosis).ToList());
    }

    public async Task<Dictionary<string, List<NoteDto>>> GetNotesByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        var data = await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Notes.Select(n => new
            {
                EncounterId = e.Id,
                Note = new NoteDto(n.Id, n.Text, n.CreatedAt)
            }))
            .ToListAsync(cancellationToken);

        return data
            .GroupBy(x => x.EncounterId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Note).ToList());
    }

    public async Task<Dictionary<string, List<PrescriptionDto>>> GetPrescriptionsByEncounterIdsAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        var data = await context.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Prescriptions.Select(p => new
            {
                EncounterId = e.Id,
                Prescription = new PrescriptionDto(p.Id, p.MedicationName, p.Dosage, p.Instructions)
            }))
            .ToListAsync(cancellationToken);

        return data
            .GroupBy(x => x.EncounterId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Prescription).ToList());
    }
}