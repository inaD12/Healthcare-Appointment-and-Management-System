namespace Shared.Application.Models;

public class GetUsersByIdsResponse
{
    public List<UserNameModel> Users { get; set; } = new();
}
