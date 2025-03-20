using Bogus;

namespace Shared.Domain.Utilities;

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

	public static string GetAverageString(int minLength, int maxLength)
	{
		var result = _faker.Random.AlphaNumeric(GetAverageNumber(minLength, maxLength));

		return result;
	}
	public static int GetAverageNumber(int maxLength, int minLength)
	{
		var result = (maxLength + minLength) / 2;

		return result;
	}

	public static long GetAverageLong(int minLength, int maxLength)
	{
		if (minLength < 1)
			minLength = 1;
		if (maxLength > 18)
			maxLength = 18;

		long minNumber = minLength == 1 ? 0 : (long)Math.Pow(10, minLength - 1);
		long maxNumber = (long)Math.Pow(10, maxLength) - 1;

		return _faker.Random.Long(minNumber, maxNumber);
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
		var result = DateTime.Now.ToUniversalTime();

		return result;
	}

	public static DateTime GetDatePast()
	{
		var result = _faker.Date.Past().ToUniversalTime();

		return result;
	}

	public static DateTime GetDateSoon()
	{
		var result = _faker.Date.Soon().ToUniversalTime();

		return result;
	}

	public static DateTime GetDateFuture()
	{
		var result = _faker.Date.Future().ToUniversalTime();

		return result;
	}
}
