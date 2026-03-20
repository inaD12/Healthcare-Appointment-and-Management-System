using Doctors.Application.Features.Doctors.Dtos;

namespace Doctors.Application.Features.Doctors.Models;

public sealed record DoctorQueryViewModel(
	string Id,
	string FirstName,
	string LastName,
	string UserId,
	string Bio,
	string TimeZoneId,
	List<string> Specialities,
	List<WorkDayDto> WorkDays,
	List<DoctorAvailabilityExceptionDto> AvailabilityExceptions,
	double AverageRating,
	int RatingsCount);
