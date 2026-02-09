using Patients.Domain.Utilities;
using Patients.Domain.ValueObjects;
using Shared.Domain.Entities.Base;
using Shared.Domain.Results;

namespace Patients.Domain.Entities;

public sealed class Patient : BaseConcurrencyEntity
{
    private readonly List<Allergy> _allergies = new();
    private readonly List<ChronicCondition> _conditions = new();

    private Patient() { }

    public Patient(string id, string firstName, string lastName, DateOnly birthDate)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateOnly BirthDate { get; private set; }

    public IReadOnlyCollection<Allergy> Allergies => _allergies;
    public IReadOnlyCollection<ChronicCondition> Conditions => _conditions;


    public Result AddAllergy(string substance, string reaction)
    {
        if (_allergies.Any(a => a.Substance == substance))
            return Result.Failure(ResponseList.AllergyAlreadyAdded);

        _allergies.Add(new Allergy(substance, reaction));
        return Result.Success();
    }

    public Result AddChronicCondition(string name)
    {
        if (_conditions.Any(c => c.Name == name))
            return Result.Failure(ResponseList.ConditionAlreadyAdded);

        _conditions.Add(new ChronicCondition(name));
        return Result.Success();
    }
}