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

        builder.Property(e => e.PatientId).IsRequired();
        builder.Property(e => e.DoctorId).IsRequired();
        builder.Property(e => e.AppointmentId).IsRequired();

        builder.HasIndex(e => e.AppointmentId).IsUnique();

        builder.Property(e => e.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.StartedAt).IsRequired();
        builder.Property(e => e.FinalizedAt);
        builder.Property(e => e.LockedAt);

        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .HasColumnName("xmin");

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(e => e.Notes, n =>
        {
            n.ToTable("EncounterNotes");

            n.WithOwner().HasForeignKey("EncounterId");

            n.HasKey(x => x.Id);

            n.Property(x => x.Id).IsRequired();
            n.Property(x => x.Text).IsRequired();
            n.Property(x => x.CreatedAt).IsRequired();
        });

        builder.OwnsMany(e => e.Diagnoses, d =>
        {
            d.ToTable("EncounterDiagnoses");

            d.WithOwner().HasForeignKey("EncounterId");

            d.HasKey(x => x.Id);

            d.Property(x => x.IcdCode).IsRequired();
            d.Property(x => x.Description).IsRequired();
        });

        builder.OwnsMany(e => e.Prescriptions, p =>
        {
            p.ToTable("EncounterPrescriptions");

            p.WithOwner().HasForeignKey("EncounterId");

            p.HasKey(x => x.Id);

            p.Property(x => x.MedicationName).IsRequired();
            p.Property(x => x.Dosage).IsRequired();
            p.Property(x => x.Instructions).IsRequired();
        });

        builder.OwnsMany(e => e.Addendums, a =>
        {
            a.ToTable("EncounterAddendums");

            a.WithOwner().HasForeignKey("EncounterId");

            a.HasKey(x => x.Id);

            a.Property(x => x.Text).IsRequired();
            a.Property(x => x.CreatedAt).IsRequired();
        });
    }
}