using Doctors.Application.Features.Doctors.Dtos;

namespace Doctors.API.Doctors.Models.Responses;

public sealed record WorkDayResponse(DayOfWeek DayOfWeek, List<WorkTimeRangeResponse> WorkTimes);