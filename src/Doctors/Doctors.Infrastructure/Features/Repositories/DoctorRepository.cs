using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Doctors.Infrastructure.Features.Repositories;

public class DoctorRepository: GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(DbContext context) : base(context)
    {
    }
}