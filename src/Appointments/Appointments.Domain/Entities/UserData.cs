using Appointments.Domain.Entities.Base;

namespace Appointments.Domain.Entities
{
	public class UserData : BaseEntity
	{
		public string UserId { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
	}
}
