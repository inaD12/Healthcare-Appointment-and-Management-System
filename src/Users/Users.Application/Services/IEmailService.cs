using Users.Domain.Result;

namespace Users.Application.Services
{
	public interface IEmailService
	{
		Task<Result> Handle(string tokenId);
	}
}