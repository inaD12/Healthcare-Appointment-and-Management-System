using Shared.Domain.Enums;

namespace Shared.API.Models.Requests;

public class CollectionReadRequest
{
	public SortOrder? SortOrder { get; set; } 

	public string? SortPropertyName { get; set; }

	public int? Page { get; set; }

	public int? PageSize { get; set; }
}
