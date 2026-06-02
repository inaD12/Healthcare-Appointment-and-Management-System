using Doctors.Application.Features.Specialities.Abstractions;
using Doctors.Application.Features.Specialities.Dtos;
using Doctors.Application.Features.Specialities.Models;
using Doctors.Domain.Entities;
using Doctors.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Shared.Application.Models;
using Shared.Infrastructure.Extensions;
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

    public async Task<(List<Speciality> Found, List<string> Missing)> GetByNamesAsync(
        IEnumerable<string> names, 
        CancellationToken cancellationToken = default)
    {
        var normalizedNames = names
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var found = await _context.Specialities
            .Where(s => normalizedNames.Contains(s.Name))
            .ToListAsync(cancellationToken);

        var foundNames = found.Select(s => s.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missing = normalizedNames.Except(foundNames, StringComparer.OrdinalIgnoreCase).ToList();

        return (found, missing);
    }
    
    public async Task<List<SpecialityMatch>?> GetNearestAsync(
        Vector embedding,
        CancellationToken cancellationToken = default)
    {
        var results = await _context.Specialities
            .Select(s => new
            {
                Speciality = s,
                Distance = s.Embedding!.CosineDistance(embedding)
            })
            .OrderBy(x => x.Distance)
            .Take(5)
            .ToListAsync(cancellationToken);

        if (results.Count == 0)
            return null;
        
        return results
            .Select(r => new SpecialityMatch(r.Speciality, r.Distance))
            .ToList();
    }

    public async Task<PagedList<Speciality>?> GetAllAsync(SpecialityPagedListQuery query, CancellationToken cancellationToken = default)
    {
        var entitiesQuery = _context.Specialities
            .AsNoTracking()
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.Name))
            entitiesQuery = entitiesQuery.Where(d => EF.Functions.ILike(d.Name, $"{query.Name}%"));
        if (!string.IsNullOrWhiteSpace(query.Description))
            entitiesQuery = entitiesQuery.Where(d => EF.Functions.ILike(d.Description, $"{query.Description}%")); ;

        entitiesQuery = entitiesQuery.ApplySorting(query.SortPropertyName, query.SortOrder);

        if (!await entitiesQuery.AnyAsync(cancellationToken))
            return null;

        var specialities = await PagedList<Speciality>.CreateAsync(
            entitiesQuery,
            query.Page,
            query.PageSize,
            cancellationToken
        );

        return specialities;
    }
}