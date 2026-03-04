using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Patients.Domain.ValueObjects;

namespace Patients.Infrastructure.Features.Configurations;

internal sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Ignore(e => e.Allergies);
        builder.Ignore(e => e.Conditions);
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();;
        
        builder.Property(p => p.UserId)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();;

        builder.Property(p => p.FirstName)
            .HasMaxLength(PatientsBusinessConfiguration.FIRSTNAME_MAX_LENGTH)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasMaxLength(PatientsBusinessConfiguration.LASTNAME_MAX_LENGTH)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .IsRequired();
        
        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.OwnsMany<Allergy>("_allergies", a =>
        {
            a.ToTable("PatientAllergies");
            
            a.WithOwner().HasForeignKey("PatientId");

            a.HasKey(x => x.Id);

            a.Property(x => x.Id)
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
                .ValueGeneratedNever();

            a.Property(x => x.Substance)
                .HasMaxLength(PatientsBusinessConfiguration.SUBSTANCE_MAX_LENGTH)
                .IsRequired();

            a.Property(x => x.Reaction)
                .HasMaxLength(PatientsBusinessConfiguration.REACTION_MAX_LENGTH)
                .IsRequired();
        });

        builder.OwnsMany<ChronicCondition>("_conditions", c =>
        {
            c.ToTable("PatientConditions");
            
            c.WithOwner().HasForeignKey("PatientId");

            c.HasKey(x => x.Id);

            c.Property(x => x.Id)
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
                .ValueGeneratedNever();

            c.Property(x => x.Name)
                .HasMaxLength(PatientsBusinessConfiguration.CHRONIC_CONDITION_NAME_MAX_LENGTH)
                .IsRequired();
        });

        builder.Navigation("_allergies")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation("_conditions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}