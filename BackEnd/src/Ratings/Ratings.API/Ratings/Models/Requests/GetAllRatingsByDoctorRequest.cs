using Shared.API.Models.Requests;
using Shared.Domain.Enums;

namespace Ratings.API.Ratings.Models.Requests;

public sealed record GetAllRatingsByDoctorRequest(
	string PatientId = "",
	string AppointmentId = "",
	int? MinScore = null,
	int? MaxScore = null,
	SortOrder SortOrder = SortOrder.ASC,
	string SortPropertyName = "CreatedAt",
	int Page = 1,
	int PageSize = 10) : CollectionReadRequest(SortOrder, SortPropertyName, Page, PageSize);