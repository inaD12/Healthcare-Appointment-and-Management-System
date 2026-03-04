using Doctors.Domain.Entities;

namespace Doctors.Domain.Dtos;

public sealed record SpecialityMatch(Speciality Speciality, double Distance);