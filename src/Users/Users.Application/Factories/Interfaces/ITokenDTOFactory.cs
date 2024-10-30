using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories.Interfaces
{
    public interface ITokenDTOFactory
    {
        TokenDTO CreateToken(string token);
    }
}