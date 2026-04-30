namespace Ratings.Application.Features.Ratings.Models;

public sealed record RatingQueryViewModel(
	string Id,
	string DoctorId,
	string PatientId,
	string AppointmentId,
	int Score,
	DateTime CreatedAt,
	string? Comment);