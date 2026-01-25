using Ratings.Domain.Utilities;
using Shared.Domain.Entities.Base;
using Shared.Domain.Results;

namespace Ratings.Domain.Entities;

public sealed class Rating: BaseEntity
{
    public string DoctorId { get; private set; }
    public string PatientId { get; private set; }
    public string AppointmentId { get; private set; }

    public int Score { get; private set; }
    public string? Comment { get; private set; }


    private Rating() { }

    private Rating(
        string doctorId,
        string patientId,
        string appointmentId,
        int score,
        string? comment)
    {
        DoctorId = doctorId;
        PatientId = patientId;
        AppointmentId = appointmentId;
        Score = score;
        Comment = comment;
    }

    public static Result<Rating> Create(
        string doctorId,
        string patientId,
        string appointmentId,
        int score,
        string? comment)
    {
        if (score < 1 || score > 5)
            return Result<Rating>.Failure(ResponseList.InvalidRatingScore);

        return Result<Rating>.Success(new Rating(
            doctorId,
            patientId,
            appointmentId,
            score,
            comment));
    }
}
