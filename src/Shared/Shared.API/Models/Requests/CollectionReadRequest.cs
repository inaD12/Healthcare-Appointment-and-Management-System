using Shared.Domain.Enums;

namespace Shared.API.Models.Requests;

public record CollectionReadRequest
(
	SortOrder SortOrder = SortOrder.ASC,
	string SortPropertyName = "Id",
	int Page = 1,
	int PageSize = 10
);
