using Doctors.Application.Features.Doctors.Mappers;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Infrastructure.Abstractions;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Serilog;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.RecommendSpeciality;

public sealed class RecommendSpecialityCommandHandler(
    IEmbeddingClient embeddingClient,
    ISpecialityRepository  specialityRepository)
    : ICommandHandler<RecommendSpecialityCommand, List<SpecialityViewModel>>
{
    public async Task<Result<List<SpecialityViewModel>>> Handle(RecommendSpecialityCommand request, CancellationToken cancellationToken)
    {
        var embedding = await embeddingClient.GenerateEmbeddingAsync(
            request.Symptoms, 
            cancellationToken
        );

        var matches = await specialityRepository.GetNearestAsync(
            embedding, 
            cancellationToken
        );

        if (matches == null || matches.Count == 0)
        {
            Log.Error("No specialities found in RecommendSpecialityCommandHandler");
            return Result<List<SpecialityViewModel>>.Failure(ResponseList.NoSpecialities);
        }
        
        List<SpecialityViewModel> validSpecialities = new List<SpecialityViewModel>();

        foreach (var speciality in matches)
        {
            if (speciality.Distance <= 0.45)
            {
                validSpecialities.Add(speciality.ToViewModel());
            }
        }

        if (validSpecialities.Count == 0)
        {
            return Result<List<SpecialityViewModel>>.Failure(ResponseList.NoCloseSpecialities);
        }
        
        return Result<List<SpecialityViewModel>>.Success(validSpecialities);
    }
}
