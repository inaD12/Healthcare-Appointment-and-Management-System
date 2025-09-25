namespace Users.Domain.Entities;

public sealed class Permission
{
    public static readonly Permission GetUser = new("users:read");
    public static readonly Permission ModifyUser = new("users:update");
    public static readonly Permission DeleteUser = new("users:delete");
    
    public static readonly Permission CreateAppointment = new("appointment:create");
    public static readonly Permission CancelAppointment = new("appointment:cancel");
    public static readonly Permission RescheduleAppointment = new("appointment:reschedule");
    public static readonly Permission GetAppointment = new("appointment:read");

    public Permission(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
