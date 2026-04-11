using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class EncounterRepository(IDbContextFactory<PatientsDbContext> factory)
    : GenericFactoryRepository<PatientsDbContext, Encounter>(factory), IEncounterRepository
{
    public async Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default)
    {
        await using var db = CreateDbContext();

        return await db.Encounters
            .SingleOrDefaultAsync(e => e.AppointmentId == appointmentId, cancellationToken);
    }

    public async Task<List<EncounterListItemDto>> GetByPatientIdsAsync(
        IReadOnlyList<string> patientIds,
        CancellationToken cancellationToken)
    {
        await using var db = CreateDbContext();

        return await db.Encounters
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
        await using var db = CreateDbContext();

        var encounters = await db.Encounters
            .Where(e => appointmentIds.Contains(e.AppointmentId))
            .ToListAsync(cancellationToken);

        return encounters.Select(e => new EncounterDetailsDto(
            e.Id,
            e.AppointmentId,
            e.DoctorId,
            e.PatientId,
            e.StartedAt,
            e.FinalizedAt,
            e.Status,
            e.Notes.Select(n => new NoteDto(n.Id, e.Id, n.Text, n.CreatedAt, n.DeletedAt)).ToList(),
            e.Diagnoses.Select(d => new DiagnosisDto(d.Id, e.Id, d.IcdCode, d.Description, d.CreatedAt, d.DeletedAt)).ToList(),
            e.Prescriptions.Select(p => new PrescriptionDto(p.Id, e.Id, p.MedicationName, p.Dosage, p.Instructions, p.CreatedAt, p.DeletedAt)).ToList(),
            e.Addendums.Select(a => new AddendumDto(a.Id, e.Id, a.Text, a.CreatedAt, a.DeletedAt)).ToList()
        )).ToList();
    }

    public async Task<List<AddendumDto>> GetAddendumsByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        await using var db = CreateDbContext();

        return await db.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Addendums.Select(a => new AddendumDto(
                a.Id,
                e.Id,
                a.Text,
                a.CreatedAt,
                a.DeletedAt)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DiagnosisDto>> GetDiagnosesByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        await using var db = CreateDbContext();

        return await db.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Diagnoses.Select(d => new DiagnosisDto(
                d.Id,
                e.Id,
                d.IcdCode,
                d.Description,
                d.CreatedAt,
                d.DeletedAt)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<NoteDto>> GetNotesByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        await using var db = CreateDbContext();

        return await db.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Notes.Select(n => new NoteDto(
                n.Id,
                e.Id,
                n.Text,
                n.CreatedAt,
                n.DeletedAt)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PrescriptionDto>> GetPrescriptionsByEncounterIdsFlatAsync(
        IReadOnlyList<string> encounterIds,
        CancellationToken cancellationToken)
    {
        await using var db = CreateDbContext();

        return await db.Encounters
            .AsNoTracking()
            .Where(e => encounterIds.Contains(e.Id))
            .SelectMany(e => e.Prescriptions.Select(p => new PrescriptionDto(
                p.Id,
                e.Id,
                p.MedicationName,
                p.Dosage,
                p.Instructions,
                p.CreatedAt,
                p.DeletedAt)))
            .ToListAsync(cancellationToken);
    }
}