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
            a.WithOwner().HasForeignKey("PatientId");

            a.Property<string>("Id")
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

            a.HasKey("Id");

            a.Property(x => x.Substance)
                .HasMaxLength(PatientsBusinessConfiguration.SUBSTANCE_MAX_LENGTH)
                .IsRequired();

            a.Property(x => x.Reaction)
                .HasMaxLength(PatientsBusinessConfiguration.REACTION_MAX_LENGTH)
                .IsRequired();
        });

        builder.OwnsMany<ChronicCondition>("_conditions", c =>
        {
            c.WithOwner().HasForeignKey("PatientId");

            c.Property<string>("Id")
                .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH);

            c.HasKey("Id");

            c.Property(x => x.Name)
                .HasMaxLength(PatientsBusinessConfiguration.CHRONIC_CONDITION_NAME_MAX_LENGTH)
                .IsRequired();
        });

        builder.Navigation(p => p.Allergies)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(p => p.Conditions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}