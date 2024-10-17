using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories
{
	public interface IEntityFactory
	{
		TokenDTO CreateToken(string token);
	}
}