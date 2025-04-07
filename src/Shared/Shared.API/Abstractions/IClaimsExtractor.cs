using Shared.API.Models;
using Shared.Utilities;

namespace Shared.API.Abstractions;

public interface IClaimsExtractor
{
	string GetUserId();
	string GetUserRole();
	ClaimsExtractorModel GetAllClaims();
	string GetUserId(string Token);
	string GetUserRole(string Token);
}