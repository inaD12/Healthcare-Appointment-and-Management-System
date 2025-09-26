namespace Users.Users.Models.Requests;

public class RegisterUserRequest
{
	public string Email { get; set; }
	public string Password { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string PhoneNumber { get; set; }
	public string Address { get; set; }
	public Roles Role { get; set; }
}
