using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Doctors.Infrastructure.Features.Repositories;

public class SpecialityRepository: GenericRepository<Speciality>, ISpecialityRepository
{
    private readonly DoctorsDbContext _context;
    
    public SpecialityRepository(DoctorsDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Speciality?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var speciality = await _context.Specialities.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

        return speciality;
    }

    public async Task<List<Speciality>> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        if (names == null || !names.Any())
            return new List<Speciality>();

        var normalizedNames = names
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return await _context.Specialities
            .Where(s => normalizedNames.Contains(s.Name))
            .ToListAsync(cancellationToken);
    }

}