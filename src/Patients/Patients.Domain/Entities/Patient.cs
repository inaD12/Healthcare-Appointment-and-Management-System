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

    private Patient(string userId, string firstName, string lastName, DateOnly birthDate)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }

    public string UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateOnly BirthDate { get; private set; }

    public IReadOnlyCollection<Allergy> Allergies => _allergies;
    public IReadOnlyCollection<ChronicCondition> Conditions => _conditions;

    public static Patient Register(
        string id,
        string firstName,
        string lastName,
        DateOnly birthDate)
        => new(id, firstName, lastName, birthDate);
    
    public Result<string> AddAllergy(string substance, string reaction)
    {
        if (_allergies.Any(a => a.Substance == substance))
            return Result<string>.Failure(ResponseList.AllergyAlreadyAdded);

        var allergy = new Allergy(substance, reaction);
        
        _allergies.Add(allergy);
        return Result<string>.Success(allergy.Id);
    }

    public Result<string> AddChronicCondition(string name)
    {
        if (_conditions.Any(c => c.Name == name))
            return Result<string>.Failure(ResponseList.ConditionAlreadyAdded);

        var condition = new ChronicCondition(name);
        
        _conditions.Add(condition);
        return Result<string>.Success(condition.Id);
    }
    
    public Result RemoveAllergy(string allergyId)
    {
        var allergy = _allergies.FirstOrDefault(n => n.Id == allergyId);
        if (allergy is null)
            return Result.Failure(ResponseList.AllergyNotFound);
        
        _allergies.Remove(allergy);
        return Result.Success();
    }
    
    public Result RemoveCondition(string conditionId)
    {
        var condition = _conditions.FirstOrDefault(n => n.Id == conditionId);
        if (condition is null)
            return Result.Failure(ResponseList.ConditionNotFound);
        
        _conditions.Remove(condition);
        return Result.Success();
    }
}