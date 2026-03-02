using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Patients.Domain.ValueObjects;

namespace Patients.Infrastructure.Features.Configurations;

internal sealed class EncounterConfiguration : IEntityTypeConfiguration<Encounter>
{
    public void Configure(EntityTypeBuilder<Encounter> builder)
    {
        builder.Ignore(e => e.Notes);
        builder.Ignore(e => e.Diagnoses);
        builder.Ignore(e => e.Prescriptions);
        builder.Ignore(e => e.Addendums);
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(e => e.PatientId)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(e => e.DoctorId)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(e => e.AppointmentId)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.HasIndex(e => e.AppointmentId)
            .IsUnique();

        builder.Property(e => e.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();
        
        builder.Property(e => e.StartedAt).IsRequired();
        builder.Property(e => e.FinalizedAt);
        builder.Property(e => e.LockedAt);

        builder.OwnsMany<ClinicalNote>("_notes", n =>
        {
            n.ToTable("EncounterNotes");
            
            n.WithOwner().HasForeignKey("EncounterId");

            n.HasKey(x => x.Id);

            n.Property(x => x.Id)
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
                .ValueGeneratedNever();

            n.Property(x => x.Text)
                .HasMaxLength(PatientsBusinessConfiguration.CLINICAL_NOTE_TEXT_MAX_LENGTH)
                .IsRequired();

            n.Property(x => x.CreatedAt)
                .IsRequired();
        });

        builder.OwnsMany<Diagnosis>("_diagnoses", d =>
        {
            d.ToTable("EncounterDiagnoses");
            
            d.WithOwner().HasForeignKey("EncounterId");

            d.HasKey(x => x.Id);

            d.Property(x => x.Id)
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
                .ValueGeneratedNever();

            d.Property(x => x.IcdCode)
                .HasMaxLength(PatientsBusinessConfiguration.ICD_MAX_LENGTH)
                .IsRequired();

            d.Property(x => x.Description)
                .HasMaxLength(PatientsBusinessConfiguration.DIAGNOSIS_DESCTIPTION_MAX_LENGTH)
                .IsRequired();
        });

        builder.OwnsMany<Prescription>("_prescriptions", p =>
        {
            p.ToTable("EncounterPrescriptions");
            
            p.WithOwner().HasForeignKey("EncounterId");

            p.HasKey(x => x.Id);

            p.Property(x => x.Id)
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
                .ValueGeneratedNever();

            p.Property(x => x.MedicationName)
                .HasMaxLength(PatientsBusinessConfiguration.PRESCRIPTION_NAME_MAX_LENGTH)
                .IsRequired();

            p.Property(x => x.Dosage)
                .HasMaxLength(PatientsBusinessConfiguration.PRESCRIPTION_DOSAGE_MAX_LENGTH)
                .IsRequired();

            p.Property(x => x.Instructions)
                .HasMaxLength(PatientsBusinessConfiguration.PRESCRIPTION_INSTRUCTIONS_MAX_LENGTH)
                .IsRequired();
        });

        builder.OwnsMany<AddendumNote>("_addendums", a =>
        {
            a.ToTable("EncounterAddendums");
            
            a.WithOwner().HasForeignKey("EncounterId");

            a.HasKey(x => x.Id);

            a.Property(x => x.Id)
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
                .ValueGeneratedNever();

            a.Property(x => x.Text)
                .HasMaxLength(PatientsBusinessConfiguration.ADDENDUM_NOTE_TEXT_MAX_LENGTH)
                .IsRequired();

            a.Property(x => x.CreatedAt)
                .IsRequired();
        });

        builder.Navigation("_notes")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_diagnoses")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_prescriptions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_addendums")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
