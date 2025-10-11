using Shared.Domain.Entities.Base;

namespace Doctors.Domain.Entities;

public sealed class Speciality : BaseEntity
{
    public string Name { get; private set; }

    private Speciality()
    {
    }

    public Speciality(string name)
    {
        Name = name;
    }
}