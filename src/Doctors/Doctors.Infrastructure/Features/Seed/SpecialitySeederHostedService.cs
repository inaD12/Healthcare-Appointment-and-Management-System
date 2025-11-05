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

        var namesToAdd = Specialities.MedicalSpecialties
            .Except(existingNames, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (namesToAdd.Count == 0)
        {
            Log.Information("Specialities already seeded.");
            return;
        }

        Log.Information($"Seeding {namesToAdd.Count} specialities...");

        var tasks = namesToAdd.Select(async name =>
        {
            var embedding = await embeddingClient.GenerateEmbeddingAsync(name, cancellationToken);
            return Speciality.Create(name, embedding);
        });

        var specialities = await Task.WhenAll(tasks);

        await context.Specialities.AddRangeAsync(specialities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Log.Information("Specialities seeded successfully.");
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}