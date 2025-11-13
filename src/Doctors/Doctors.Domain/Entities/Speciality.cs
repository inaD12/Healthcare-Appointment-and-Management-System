using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;
using Shared.Domain.Entities.Base;

namespace Doctors.Domain.Entities;

public sealed class Speciality : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    [Column(TypeName = "vector(1024)")]
    public Vector Embedding { get; set; }

    private Speciality() {}

    private Speciality(string name, string description, Vector embedding)
    {
        Name = name;
        Description = description;
        Embedding = embedding;
    }

    public static Speciality Create(string name, string description, Vector embedding)
    {
        return new Speciality(name, description, embedding);
    }
    
    public override string ToString() => Name;
}