using Appointments.Domain.Entities.Base;
using Contracts.Enums;

namespace Appointments.Domain.Entities
{
	public class UserData : BaseEntity
	{
		public string UserId { get; set; }
		public string Email { get; set; }
		public Roles Role { get; set; }
	}
}
