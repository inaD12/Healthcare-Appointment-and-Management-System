namespace Users.Domain.Entities
{
	public class User
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public string Salt { get; set; }
		public string Role { get; set; } // Patient, Doctor, Admin
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
	}
}
