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
            n.WithOwner().HasForeignKey("EncounterId");

            n.Property<string>("Id")
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

            n.HasKey("Id");

            n.Property(x => x.Text)
                .HasMaxLength(PatientsBusinessConfiguration.CLINICAL_NOTE_TEXT_MAX_LENGTH)
                .IsRequired();

            n.Property(x => x.CreatedAt)
                .IsRequired();
        });

        builder.OwnsMany<Diagnosis>("_diagnoses", d =>
        {
            d.WithOwner().HasForeignKey("EncounterId");

            d.Property<string>("Id")
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

            d.HasKey("Id");

            d.Property(x => x.IcdCode)
                .HasMaxLength(PatientsBusinessConfiguration.ICD_MAX_LENGTH)
                .IsRequired();

            d.Property(x => x.Description)
                .HasMaxLength(PatientsBusinessConfiguration.DIAGNOSIS_DESCTIPTION_MAX_LENGTH)
                .IsRequired();
        });

        builder.OwnsMany<Prescription>("_prescriptions", p =>
        {
            p.WithOwner().HasForeignKey("EncounterId");

            p.Property<string>("Id")
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

            p.HasKey("Id");

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
            a.WithOwner().HasForeignKey("EncounterId");

            a.Property<string>("Id")
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

            a.HasKey("Id");

            a.Property(x => x.Text)
                .HasMaxLength(PatientsBusinessConfiguration.ADDENDUM_NOTE_TEXT_MAX_LENGTH)
                .IsRequired();

            a.Property(x => x.CreatedAt)
                .IsRequired();
        });

        builder.Navigation(e => e.Notes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Diagnoses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Prescriptions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Addendums)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
