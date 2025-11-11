using Doctors.Application.Features.Doctors.Models;

namespace Doctors.API.Doctors.Models.Responses;

public sealed record RecommendSpecialityResponse(List<SpecialityViewModel> Specialities);