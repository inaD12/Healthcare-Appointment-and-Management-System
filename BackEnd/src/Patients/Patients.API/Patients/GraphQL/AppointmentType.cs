using Patients.API.Patients.GraphQL.DataLoaders;
using Patients.Domain.Entities;

namespace Patients.API.Patients.GraphQL;

public class AppointmentType : ObjectType<AppointmentProjection>
{
    protected override void Configure(IObjectTypeDescriptor<AppointmentProjection> descriptor)
    {
        descriptor.Field(x => x.Id);

        descriptor
            .Field("doctorName")
            .Type<StringType>()
            .Resolve(async (ctx, ct) =>
            {
                var appointment = ctx.Parent<AppointmentProjection>();
                var loader = ctx.DataLoader<UserNamesDataLoader>();

                var names = await loader.LoadAsync(appointment.DoctorId, ct);

                return names == null
                    ? null
                    : $"{names.FirstName} {names.LastName}";
            });

        descriptor
            .Field("patientName")
            .Type<StringType>()
            .Resolve(async (ctx, ct) =>
            {
                var appointment = ctx.Parent<AppointmentProjection>();
                var loader = ctx.DataLoader<UserNamesDataLoader>();

                var names = await loader.LoadAsync(appointment.PatientId, ct);

                return names == null
                    ? null
                    : $"{names.FirstName} {names.LastName}";
            });

        descriptor
            .Field("encounterDetails")
            .Resolve(async (ctx, ct) =>
            {
                var appointment = ctx.Parent<AppointmentProjection>();
                var loader = ctx.DataLoader<EncountersByAppointmentDataLoader>();

                return await loader.LoadAsync(appointment.Id, ct);
            });
    }
}