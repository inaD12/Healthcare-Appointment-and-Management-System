namespace Ratings.API.Ratings.Models.Responses;

public sealed record RatingQueryResponse(
    string Id,
    string DoctorId,
    string PatientId,
    string AppointmentId,
    int Score,
    string? Comment);