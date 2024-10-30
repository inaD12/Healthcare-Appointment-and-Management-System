using Users.Domain.Entities;
using Users.Domain.Result;

namespace Users.Application.Helpers.Interfaces
{
    public interface IEmailVerificationSender
    {
        Task<Result> SendEmailAsync(User user);
    }
}