namespace Shared.Domain.Models;

public class GetUsersByIdsResponse
{
    public List<UserNameModel> Users { get; set; } = new();
}
