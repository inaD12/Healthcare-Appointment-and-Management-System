using System.Text.Json;
using Doctors.Domain.Abstractions;
using Doctors.Domain.Entities;
using Doctors.Infrastructure.Features.DBContexts;
using Doctors.Infrastructure.Features.Dtos;
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

        var path = Path.Combine(AppContext.BaseDirectory, "Utilities", "specialities.json");
        var json = await File.ReadAllTextAsync(path, cancellationToken);
        var data = JsonSerializer.Deserialize<List<SpecialitySeed>>(json);
        
        if (data is null)
        {
            Log.Error("No data found for speciality seeder");
            return;
        }
        
        var specialtiesToAdd = data
            .Where(speciality => !existingNames.Contains(speciality.name, StringComparer.OrdinalIgnoreCase))
            .ToList();

        if (specialtiesToAdd.Count == 0)
        {
            Log.Information("Specialities already seeded.");
            return;
        }

        Log.Information($"Seeding {specialtiesToAdd.Count} specialities...");

        var tasks = specialtiesToAdd.Select(async speciality =>
        {
            var embedding = await embeddingClient.GenerateEmbeddingAsync(speciality.description, cancellationToken);
            return Speciality.Create(speciality.name, speciality.description, embedding);
        });

        var specialities = await Task.WhenAll(tasks);

        await context.Specialities.AddRangeAsync(specialities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Log.Information("Specialities seeded successfully.");
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}