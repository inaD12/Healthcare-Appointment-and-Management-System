using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories
{
	public class EntityFactory : IEntityFactory
	{
		public TokenDTO CreateToken(string token)
		{
			return new TokenDTO { Token = token };
		}
	}
}
