namespace Doctors.API.Doctors.Models.Requests;

public sealed record WorkTimeRangeModel(TimeOnly Start, TimeOnly End);