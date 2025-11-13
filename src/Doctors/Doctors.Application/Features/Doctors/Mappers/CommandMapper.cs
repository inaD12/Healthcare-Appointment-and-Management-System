using Doctors.Application.Features.Doctors.Dtos;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Application.Features.Doctors.Queries.GetAllDoctors;
using Doctors.Domain.Dtos;
using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Models;
using Shared.Domain.Models;

namespace Doctors.Application.Features.Doctors.Mappers;

public static class CommandMapper
{
    public static SpecialityViewModel ToViewModel(
        this SpecialityMatch specialityMatch)
        => new(
            specialityMatch.Speciality.Name);
}