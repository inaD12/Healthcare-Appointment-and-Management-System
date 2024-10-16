using System.ComponentModel.DataAnnotations;

namespace Users.Domain.DTOs.Requests
{
	public class RegisterReqDTO
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }

	}
}
