using Contracts.Utilities;

namespace Users.Domain.Utilities;

public class UsersTestUtilities : SharedTestUtilities
{
	public static readonly string ValidId = GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH);
	public static readonly string InvalidId = GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH);

	public static readonly string ValidFirstName = GetAverageString(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH, UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH);
	public static readonly string ValidLastName = GetAverageString(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH, UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);

	public static readonly string ValidPassword = GetAverageString(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH, UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);
	public static readonly string InvalidPassword = GetAverageString(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH, UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);
}
}
