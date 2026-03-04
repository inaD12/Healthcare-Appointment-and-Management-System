using Shared.API.Models.Requests;
using Shared.Domain.Enums;

namespace Doctors.API.Doctors.Models.Requests;

public record GetAllDoctorsRequest
(
	string FirstName = "",
	string LastName = "",
	string Speciality = "",
	string TimeZoneId  = "",
	SortOrder SortOrder = SortOrder.ASC,
	string SortPropertyName = "Id",
	int Page = 1,
	int PageSize = 10
) : CollectionReadRequest(SortOrder, SortPropertyName, Page, PageSize);
