namespace Shared.Domain.Entities.Base;

public abstract class BaseEntity
{
	public BaseEntity()
	{
		Id = Guid.NewGuid().ToString();
	}
	public string Id { get; set; }
}
