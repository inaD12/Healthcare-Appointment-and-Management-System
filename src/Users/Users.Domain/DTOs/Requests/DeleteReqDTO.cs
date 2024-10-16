using System.ComponentModel.DataAnnotations;

namespace Users.Domain.DTOs.Requests
{
	public class DeleteReqDTO
	{
		[Required]
		public string Id { get; set; }
	}
}
