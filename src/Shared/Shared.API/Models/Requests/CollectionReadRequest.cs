using Shared.Domain.Enums;

namespace Shared.API.Models.Requests;

public class CollectionReadRequest
{
	public SortOrder? SortOrder { get; set; }

	public string SortPropertyName { get; set; } = "Id";

	public int Page { get; set; } = 1;

	public int PageSize { get; set; } = 10;
}
