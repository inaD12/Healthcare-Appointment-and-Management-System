using System.ComponentModel.DataAnnotations;

namespace Users.Domain.DTOs.Requests
{
	public class LoginReqDTO
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
