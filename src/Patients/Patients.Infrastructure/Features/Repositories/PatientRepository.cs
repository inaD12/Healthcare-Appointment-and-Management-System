using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class PatientRepository(PatientsDbContext context) : GenericRepository<Patient>(context), IPatientRepository
{
}