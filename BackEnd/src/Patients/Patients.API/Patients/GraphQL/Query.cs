using Patients.Application.Features.Encounters.Queries;
using Patients.Application.Features.Patients.Queries;

namespace Patients.API.Patients.GraphQL;


public sealed class Query : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor.Field<PatientQueries>(t => t.GetPatients(default!));
        descriptor.Field<PatientQueries>(t => t.GetPatientHeader(default!, default!));

        descriptor.Field<EncounterQueries>(t => t.GetEncountersByPatient(default!, default!));
        descriptor.Field<EncounterQueries>(t => t.GetEncounterDetails(default!, default!));
    }
}
