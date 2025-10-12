using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Doctors.Infrastructure.Features.Repositories;

public class DoctorRepository: GenericRepository<Doctor>, IDoctorRepository
{
    private readonly DoctorsDbContext _context;
    public DoctorRepository(DoctorsDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Doctor?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        return doctor;
    }
}