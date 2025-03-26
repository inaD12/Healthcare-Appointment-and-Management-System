using Shared.Domain.Utilities;

namespace Users.Domain.Utilities;

public class UsersTestUtilities : SharedTestUtilities
{
	public static readonly string ValidId = GetAverageString(UsersBusinessConfiguration.ID_MIN_LENGTH, UsersBusinessConfiguration.ID_MAX_LENGTH);
	public static readonly string InvalidId = GetAverageString(UsersBusinessConfiguration.ID_MIN_LENGTH, UsersBusinessConfiguration.ID_MAX_LENGTH);

	public static readonly string ValidFirstName = GetAverageString(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH, UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH);
	public static readonly string ValidLastName = GetAverageString(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH, UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);
	public static readonly string InvalidLastName = GetAverageString(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH, UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);

	public static readonly string ValidPassword = GetAverageString(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH, UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);
	public static readonly string InvalidPassword = GetAverageString(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH, UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);

	public static readonly string ValidEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH - 12) + "@gmail.com";
	public static readonly string InvalidEmail = GetAverageString(UsersBusinessConfiguration.EMAIL_MIN_LENGTH, UsersBusinessConfiguration.EMAIL_MAX_LENGTH - 12) + "@gmail.com";

	public static readonly string ValidPhoneNumber = GetAverageLong(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH, UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH).ToString();
	public static readonly string ValidAdress = GetAverageString(UsersBusinessConfiguration.ADRESS_MIN_LENGTH, UsersBusinessConfiguration.ADRESS_MAX_LENGTH);
	public static readonly string ValidPasswordHash = GetString();
	public static readonly string ValidSalt = GetString();

	public static readonly string Link = GetString();
	public static readonly string Token = GetString();

	public static readonly DateTime PastDate = GetDatePast().ToUniversalTime();
	public static readonly DateTime CurrentDate = GetDate().ToUniversalTime();
	public static readonly DateTime SoonDate = GetDateSoon().ToUniversalTime();
}
