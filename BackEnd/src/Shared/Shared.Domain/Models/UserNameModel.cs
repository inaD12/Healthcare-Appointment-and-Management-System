namespace Shared.Domain.Models;

public class UserNameModel
{
    public string Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}