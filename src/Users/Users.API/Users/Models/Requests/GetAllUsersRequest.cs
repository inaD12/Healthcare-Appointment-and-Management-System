using Shared.API.Models.Requests;
using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Users.Models.Requests;

public record GetAllUsersRequest
(
	string Email = "",
	Roles? Role = null,
	string FirstName  = "",
	string LastName  = "",
	string PhoneNumber  = "",
	string Address = "",
	bool? EmailVerified = null,
	SortOrder SortOrder = SortOrder.ASC,
	string SortPropertyName = "Id",
	int Page = 1,
	int PageSize = 10
) : CollectionReadRequest(SortOrder, SortPropertyName, Page, PageSize);
