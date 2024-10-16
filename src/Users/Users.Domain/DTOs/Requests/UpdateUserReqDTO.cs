using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.DTOs.Requests
{
	public class UpdateUserReqDTO
	{
		public string? NewEmail { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
	}
}
