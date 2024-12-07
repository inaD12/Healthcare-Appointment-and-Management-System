using Contracts.Results;

namespace Users.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Result> HandleAsync(string tokenId);
    }
}