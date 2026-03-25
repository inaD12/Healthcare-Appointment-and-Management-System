using Patients.Application.Features.AppointmentProjections.Queries.DataLoaders;
using Patients.Domain.Dtos;

namespace Patients.API.Patients.GraphQL;

public class AppointmentHistoryType : ObjectType<AppointmentHistoryDto>
{
    protected override void Configure(IObjectTypeDescriptor<AppointmentHistoryDto> descriptor)
    {
        descriptor
            .Field(x => x.DoctorName)
            .Resolve(async (ctx, ct) =>
            {
                var appointment = ctx.Parent<AppointmentHistoryDto>();
                var loader = ctx.DataLoader<UserNamesDataLoader>();

                var names = await loader.LoadAsync(appointment.DoctorId, ct);

                return names is null
                    ? null
                    : $"{names.FirstName} {names.LastName}";
            });
    }
}