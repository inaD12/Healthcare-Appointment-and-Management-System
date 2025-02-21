using System.Security.Cryptography;
using Users.Application.Features.Auth.Abstractions;

namespace Users.Application.Features.Auth;

public class PasswordManager : IPasswordManager
{
	private const int _keySize = 64;
	private const int _iterations = 1000;
	private HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

	public string HashPassword(string password, out string salt)
	{
		byte[] saltByteArray = RandomNumberGenerator.GetBytes(_keySize);
		salt = Convert.ToHexString(saltByteArray);

		byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
			password,
			saltByteArray,
			_iterations,
			_hashAlgorithm,
			_keySize);

		return Convert.ToHexString(hash);
	}

	public bool VerifyPassword(string password, string hash, string salt)
	{
		byte[] hashFromPass = Rfc2898DeriveBytes.Pbkdf2(
			password,
			Convert.FromHexString(salt),
			_iterations,
			_hashAlgorithm,
			_keySize);

		return CryptographicOperations.FixedTimeEquals(hashFromPass, Convert.FromHexString(hash));
	}
}
