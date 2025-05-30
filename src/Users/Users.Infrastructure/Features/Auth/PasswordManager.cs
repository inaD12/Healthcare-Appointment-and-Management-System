using System.Security.Cryptography;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;

namespace Users.Infrastructure.Features.Auth;

public class PasswordManager : IPasswordManager
{
	private const int _keySize = 64;
	private const int _iterations = 1000;
	private HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

	public PasswordHashResult HashPassword(string password)
	{
		byte[] saltByteArray = RandomNumberGenerator.GetBytes(_keySize);
		string salt = Convert.ToHexString(saltByteArray);

		byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
			password,
			saltByteArray,
			_iterations,
			_hashAlgorithm,
			_keySize);

		string stringHash = Convert.ToHexString(hash);

		return new PasswordHashResult(stringHash, salt);
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
