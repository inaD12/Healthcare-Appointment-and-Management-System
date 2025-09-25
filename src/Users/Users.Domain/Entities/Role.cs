namespace Users.Domain.Entities;

public sealed class Role
{
    public static readonly Role Administrator = new("Administrator");
    public static readonly Role Patient = new("Patient");
    public static readonly Role Doctor = new("Doctor");

    private Role(string name)
    {
        Name = name;
    }

    private Role()
    {
    }

    public string Name { get; private set; }
}
