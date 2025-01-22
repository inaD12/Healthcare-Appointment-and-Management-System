using Contracts.Results;

namespace Appointments.Application.Helpers
{
	public class JWTUserExtractor : IJWTUserExtractor
	{
		private readonly IJwtParser _jwtParser;
		public JWTUserExtractor(IJwtParser jwtParser)
		{
			_jwtParser = jwtParser;
		}
		public async Task<Result<string>> GetUserIdFromTokenAsync()
		{
				var userIdRes = _jwtParser.GetIdFromToken();
				if (userIdRes.IsFailure)
					return Result<string>.Failure(userIdRes.Response);

				return Result<string>.Success(userIdRes.Value);
		}
	}
}
