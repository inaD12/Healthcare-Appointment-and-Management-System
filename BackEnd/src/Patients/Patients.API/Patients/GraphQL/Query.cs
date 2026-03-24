using Patients.Application.Features.AppointmentProjections.Queries;
using Patients.Application.Features.Encounters.Queries;
using Patients.Application.Features.Patients.Queries;

namespace Patients.API.Patients.GraphQL;


public sealed class Query : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor.Field<PatientQueries>(t => t.GetPatients(default!));
        descriptor.Field<PatientQueries>(t => t.GetMyPatientHeader(default!, default!));

        descriptor.Field<EncounterQueries>(t => t.GetMyEncounters(default!, default!));
        descriptor.Field<EncounterQueries>(t => t.GetEncounterDetails(default!, default!));
        
        descriptor.Field<AppointmentQueries>(t => t.GetAppointmentById(default!, default!));
        descriptor.Field<AppointmentQueries>(t => t.GetAppointmentsByDoctor(default!, default!));
        descriptor.Field<AppointmentQueries>(t => t.GetMyAppointments(default!, default!));
    }
}
