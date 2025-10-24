using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Queries.GetDoctorByUserId;

public sealed record GetDoctorByUserIdQuery(string Id) : IQuery<DoctorQueryViewModel>;
