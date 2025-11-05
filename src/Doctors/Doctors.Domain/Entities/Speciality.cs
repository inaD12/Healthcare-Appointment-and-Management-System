using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;
using Shared.Domain.Entities.Base;

namespace Doctors.Domain.Entities;

public sealed class Speciality : BaseEntity
{
    public string Name { get; private set; }
    [Column(TypeName = "vector(1024)")]
    public Vector Embedding { get; set; }

    private Speciality() {}

    private Speciality(string name, Vector embedding)
    {
        Name = name;
        Embedding = embedding;
    }

    public static Speciality Create(string name, Vector embedding)
    {
        return new Speciality(name, embedding);
    }
    
    public override string ToString() => Name;
}