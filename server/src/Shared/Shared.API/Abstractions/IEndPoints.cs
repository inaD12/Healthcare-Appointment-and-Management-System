using Microsoft.AspNetCore.Routing;

namespace Shared.API.Abstractions;

public interface IEndPoints
{
	void RegisterEndpoints(IEndpointRouteBuilder app);
}
