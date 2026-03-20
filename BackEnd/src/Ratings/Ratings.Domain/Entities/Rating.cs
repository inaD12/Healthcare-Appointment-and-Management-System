using FluentValidation.Results;
using Shared.Domain.Entities.Base;
using Shared.Domain.Exceptions;

namespace Ratings.Domain.Entities;

public sealed class Rating: BaseEntity
{
    public string DoctorId { get; private set; }
    public string PatientId { get; private set; }
    public string AppointmentId { get; private set; }

    public int Score { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? Comment { get; private set; }


    private Rating() { }

    private Rating(
        string doctorId,
        string patientId,
        string appointmentId,
        int score,
        DateTime createdAt,
        string? comment)
    {
        DoctorId = doctorId;
        PatientId = patientId;
        AppointmentId = appointmentId;
        Score = score;
        CreatedAt = createdAt;
        Comment = comment;
    }

    public static Rating Create(
        string doctorId,
        string patientId,
        string appointmentId,
        int score,
        DateTime createdAt,
        string? comment)
    {
        if (score < 1 || score > 5)
            throw new HamsValidationException(new[]
            {
                new ValidationFailure(
                    "Rating", "Rating score must be between 1 and 5.")
            });
        
        return new Rating(
            doctorId,
            patientId,
            appointmentId,
            score,
            createdAt,
            comment);
    }

    public void UpdateScore(int score)
    {
        if (score < 1 || score > 5)
            throw new HamsValidationException(new[]
            {
                new ValidationFailure(
                    "Rating", "Rating score must be between 1 and 5.")
            });
        
        Score = score;
    }

    public void UpdateComment(string comment)
    {
        Comment = comment;
    }
}
