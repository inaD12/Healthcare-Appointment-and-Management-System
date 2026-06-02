using Doctors.Domain.Entities;

namespace Doctors.Application.Features.Specialities.Dtos;

public sealed record SpecialityMatch(Speciality Speciality, double Distance);