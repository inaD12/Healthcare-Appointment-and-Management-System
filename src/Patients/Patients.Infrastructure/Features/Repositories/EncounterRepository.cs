using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class EncounterRepository(DbContext context) : GenericRepository<Encounter>(context), IEncounterRepository
{
}