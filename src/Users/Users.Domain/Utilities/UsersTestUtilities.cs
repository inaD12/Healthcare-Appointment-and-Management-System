using Contracts.Utilities;
using Users.Domain.DTOs.Responses;

namespace Users.Domain.Utilities;

public class UsersTestUtilities : SharedTestUtilities
{
	public static readonly string ValidId = GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string InvalidId = GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string TakenId = GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH);

	public static readonly string ValidFirstName = GetAverageString(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH, UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH);
	public static readonly string ValidLastName = GetAverageString(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH, UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);
	public static readonly string InvalidLastName = GetAverageString(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH, UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);

	public static readonly string ValidPassword = GetAverageString(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH, UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);
	public static readonly string InvalidPassword = GetAverageString(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH, UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);

	public static readonly string ValidEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH);
	public static readonly string InvalidEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH);
	public static readonly string TakenEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH);
	public static readonly string EmailSendingErrorEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH);
	public static readonly string UnusedEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH);

	public static readonly string ValidPhoneNumber = GetAverageString(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH, UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH);
	public static readonly string ValidAdress = GetAverageString(UsersBusinessConfiguration.ADRESS_MIN_LENGTH, UsersBusinessConfiguration.ADRESS_MAX_LENGTH);
	public static readonly string ValidPasswordHash = GetString();
	public static readonly string InvalidPasswordHash = GetString();
	public static readonly string ValidSalt = GetString();

	public static readonly string Link = GetString();

	public static readonly TokenDTO TokenDTO = new TokenDTO("Token");

	public static readonly DateTime PastDate = GetDatePast();
	public static readonly DateTime CurrentDate = GetDate();
	public static readonly DateTime SoonDate = GetDateSoon();
}
