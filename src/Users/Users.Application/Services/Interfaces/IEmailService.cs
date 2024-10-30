using Users.Domain.Result;

namespace Users.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Result> HandleAsync(string tokenId);
    }
}