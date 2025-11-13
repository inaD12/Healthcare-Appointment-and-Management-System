using System.Net;
using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Exceptions;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Commands.AddWorkDaySchedule;

public sealed class AddWorkDayScheduleCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddWorkDayScheduleCommand>
{
    public async Task<Result> Handle(AddWorkDayScheduleCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(request.UserId,  cancellationToken);
        if (doctor == null)
            return Result.Failure(ResponseList.DoctorNotFound);
        
        var workTimeRanges = request.WorkTimes
            .Select(t => WorkTimeRange.Create(t.Start, t.End))
            .ToList();

        WorkDay workDay;
        try
        {
            workDay = WorkDay.Create(request.DayOfWeek, workTimeRanges);
        }
        catch (HamsValidationException ex)
        {
            return Result.Failure(Response.Create(ex.Message, HttpStatusCode.Conflict));
        }

        var result = doctor.AddWorkDay(workDay);
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}