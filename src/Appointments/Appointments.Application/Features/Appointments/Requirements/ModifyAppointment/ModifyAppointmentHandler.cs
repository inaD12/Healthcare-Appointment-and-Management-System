using Appointments.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Shared.Domain.Entities;
using Shared.Infrastructure.Authentication;

namespace Appointments.Application.Features.Appointments.Requirements.ModifyAppointment;

public class ModifyAppointmentHandler : AuthorizationHandler<ModifyAppointmentRequirement, Appointment>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ModifyAppointmentRequirement requirement,
        Appointment resource)
    {
        string userId = context.User.GetUserId();
        
        if (resource.PatientId == userId || resource.DoctorId == userId)
        {
            context.Succeed(requirement);
        }
        if(context.User.IsInRole(Role.Administrator.Name))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}