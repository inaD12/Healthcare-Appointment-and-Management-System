using Shared.API.Models;

namespace Shared.API.Abstractions;

public interface IClaimsExtractor
{
	string GetUserId();
	string GetUserRole();
	ClaimsExtractorModel GetAllClaims();
}