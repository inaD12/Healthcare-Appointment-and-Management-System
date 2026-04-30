using Shared.API.Models.Requests;
using Shared.Domain.Enums;

namespace Doctors.API.Doctors.Models.Requests;

public record GetAllSpecialitiesRequest
(
	string Name = "",
	string Description = "",
	SortOrder SortOrder = SortOrder.ASC,
	string SortPropertyName = "Name",
	int Page = 1,
	int PageSize = 10
) : CollectionReadRequest(SortOrder, SortPropertyName, Page, PageSize);
