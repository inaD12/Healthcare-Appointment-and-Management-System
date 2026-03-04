using Doctors.Domain.Abstractions.Repositories;
using Doctors.Domain.Entities;
using Doctors.Domain.Models;
using Doctors.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Models;
using Shared.Infrastructure.Extensions;
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
        var doctor = await _context.Doctors
            .Include(d => d.Specialities)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        return doctor;
    }

    public override async Task<Doctor?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Specialities)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        return doctor;
    }

    public async Task<PagedList<Doctor>?> GetAllAsync(DoctorPagedListQuery query, CancellationToken cancellationToken = default)
    {
        var entitiesQuery = _context.Doctors
            .AsNoTracking()
            .Include(d => d.Specialities)
            .Include(d => d.WeeklySchedule)
                .ThenInclude(ws => ws.WorkDays)
                    .ThenInclude(wd => wd.WorkTimes)
            .Include(d => d.AvailabilityExceptions)
            .AsSplitQuery()
            .AsQueryable();

        
        if (!string.IsNullOrWhiteSpace(query.FirstName))
            entitiesQuery = entitiesQuery.Where(d => EF.Functions.ILike(d.FirstName, $"{query.FirstName}%"));
        if (!string.IsNullOrWhiteSpace(query.LastName))
            entitiesQuery = entitiesQuery.Where(d => EF.Functions.ILike(d.LastName, $"{query.LastName}%"));
        if (!string.IsNullOrWhiteSpace(query.Speciality))
            entitiesQuery = entitiesQuery.Where(d =>
                d.Specialities.Any(s => EF.Functions.ILike(s.Name, $"{query.Speciality}%")));
        if (!string.IsNullOrWhiteSpace(query.TimeZoneId))
            entitiesQuery = entitiesQuery.Where(d => EF.Functions.ILike(d.TimeZoneId, $"{query.TimeZoneId}%"));

        entitiesQuery = entitiesQuery.ApplySorting(query.SortPropertyName, query.SortOrder);

        if (!await entitiesQuery.AnyAsync(cancellationToken))
            return null;

        var doctors = await PagedList<Doctor>.CreateAsync(
            entitiesQuery,
            query.Page,
            query.PageSize,
            cancellationToken
        );

        return doctors;
    }

}