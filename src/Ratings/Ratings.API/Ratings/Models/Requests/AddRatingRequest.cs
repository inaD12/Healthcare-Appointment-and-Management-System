namespace Ratings.API.Ratings.Models.Requests;

public sealed record AddRatingRequest(
	string AppointmentId,
	int Score,
	string? Comment
);
