using Patients.API.Patients.GraphQL.DataLoaders;
using Patients.Domain.Entities;

namespace Patients.API.Patients.GraphQL;

public sealed class EncounterType : ObjectType<Encounter>
{
    protected override void Configure(IObjectTypeDescriptor<Encounter> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.PatientId);
        descriptor.Field(x => x.DoctorId);
        descriptor.Field(x => x.AppointmentId);
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.StartedAt);
        descriptor.Field(x => x.FinalizedAt);
        descriptor.Field(x => x.LockedAt);
        descriptor.Field(x => x.UpdatedAt);

        descriptor
            .Field("notes")
            .Resolve(async (ctx, ct) =>
            {
                var encounter = ctx.Parent<Encounter>();
                var loader = ctx.DataLoader<NotesByEncounterDataLoader>();

                return await loader.LoadAsync(encounter.Id, ct);
            });

        descriptor
            .Field("diagnoses")
            .Resolve(async (ctx, ct) =>
            {
                var encounter = ctx.Parent<Encounter>();
                var loader = ctx.DataLoader<DiagnosesByEncounterDataLoader>();

                return await loader.LoadAsync(encounter.Id, ct);
            });

        descriptor
            .Field("prescriptions")
            .Resolve(async (ctx, ct) =>
            {
                var encounter = ctx.Parent<Encounter>();
                var loader = ctx.DataLoader<PrescriptionsByEncounterDataLoader>();

                return await loader.LoadAsync(encounter.Id, ct);
            });

        descriptor
            .Field("addendums")
            .Resolve(async (ctx, ct) =>
            {
                var encounter = ctx.Parent<Encounter>();
                var loader = ctx.DataLoader<AddendumsByEncounterDataLoader>();

                return await loader.LoadAsync(encounter.Id, ct);
            });
        
        descriptor
            .Field("doctorName")
            .Type<StringType>()
            .Resolve(async (ctx, ct) =>
            {
                var encounter = ctx.Parent<Encounter>();
                var loader = ctx.DataLoader<UserNamesDataLoader>();

                var names = await loader.LoadAsync(encounter.DoctorId, ct);

                return names == null
                    ? null
                    : $"{names.FirstName} {names.LastName}";
            });

        descriptor
            .Field("patientName")
            .Type<StringType>()
            .Resolve(async (ctx, ct) =>
            {
                var encounter = ctx.Parent<Encounter>();
                var loader = ctx.DataLoader<UserNamesDataLoader>();

                var names = await loader.LoadAsync(encounter.PatientId, ct);

                return names == null
                    ? null
                    : $"{names.FirstName} {names.LastName}";
            });
    }
}