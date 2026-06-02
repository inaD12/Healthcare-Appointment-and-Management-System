using System.Net;
using Doctors.Application.Features.Doctors.Abstractions;
using Doctors.Domain.Entities;
using Doctors.Domain.Utilities;
using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Exceptions;
using Shared.Domain.Responses;
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