using Contracts.Results;
using Users.Domain.Entities;

namespace Users.Application.Helpers.Interfaces
{
    public interface IEmailVerificationSender
    {
        Task<Result> SendEmailAsync(User user);
    }
}