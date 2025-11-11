using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions;
using Doctors.Domain.Utilities.Strings;
using Doctors.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Doctors.Infrastructure.Features.Seed;

public class SpecialitySeederHostedService(
    IServiceScopeFactory scopeFactory,
    IEmbeddingClient embeddingClient)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();
        
        var existingNames = await context.Specialities
            .Select(s => s.Name)
            .ToListAsync(cancellationToken);

        var specialtiesToAdd = Specialities.MedicalSpecialties
            .Where(speciality => !existingNames.Contains(speciality.Key, StringComparer.OrdinalIgnoreCase))
            .ToList();

        if (specialtiesToAdd.Count == 0)
        {
            Log.Information("Specialities already seeded.");
            return;
        }

        Log.Information($"Seeding {specialtiesToAdd.Count} specialities...");

        var tasks = specialtiesToAdd.Select(async speciality =>
        {
            var embedding = await embeddingClient.GenerateEmbeddingAsync(speciality.Value, cancellationToken);
            return Speciality.Create(speciality.Key, speciality.Value, embedding);
        });

        var specialities = await Task.WhenAll(tasks);

        await context.Specialities.AddRangeAsync(specialities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Log.Information("Specialities seeded successfully.");
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}