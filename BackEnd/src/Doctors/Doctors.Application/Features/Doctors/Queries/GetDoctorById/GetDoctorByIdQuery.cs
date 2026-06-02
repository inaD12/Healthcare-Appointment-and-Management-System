using Doctors.Application.Features.Doctors.Models;
using Shared.Application.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Queries.GetDoctorById;

public sealed record GetDoctorByIdQuery(string Id) : IQuery<DoctorQueryViewModel>;
