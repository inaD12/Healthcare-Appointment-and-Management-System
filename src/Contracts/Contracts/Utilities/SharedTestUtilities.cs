using Bogus;

namespace Contracts.Utilities;

public class SharedTestUtilities
{
	private static readonly Faker _faker = new();

	private const int DefaultStringLength = 30;
	public static string GetGuid()
	{
		var result = _faker.Random.Guid().ToString();

		return result;
	}

	public static string GetString(int length = DefaultStringLength)
	{
		var result = _faker.Random.AlphaNumeric(length);

		return result;
	}

	public static string GetAverageString(int maxLength, int minLength)
	{
		var result = _faker.Random.AlphaNumeric(GetAverageNumber(maxLength, minLength));

		return result;
	}

	public static int GetAverageNumber(int maxLength, int minLength)
	{
		var result = (maxLength + minLength) / 2;

		return result;
	}

	public static string GetHalfStartString(string value)
	{
		var result = value[..(value.Length / 2)];

		return result;
	}

	public static string GetNonCaseMatchingString(string value)
	{
		var result = value.ToUpper();

		return result;
	}

	public static DateTime GetDate()
	{
		var result = _faker.Date.Soon();

		return result;
	}

	public static DateTime GetDatePast()
	{
		var result = _faker.Date.Past();

		return result;
	}

	public static DateTime GetDateSoon()
	{
		var result = _faker.Date.Soon();

		return result;
	}

	public static DateTime GetDateFuture()
	{
		var result = _faker.Date.Future();

		return result;
	}
}
