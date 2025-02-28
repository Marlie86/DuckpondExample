using System.Security.Cryptography;

namespace DuckpondExample.Shared.Common.Hosts.Utilities;
/// <summary>
/// Utility class for hashing and verifying passwords using PBKDF2 with HMAC-SHA512.
/// </summary>
public class HashUtility
{
    private static readonly int SaltSize = 16;
    private static readonly int Iterations = 10000;
    private static readonly int KeySize = 20;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA512;

    /// <summary>
    /// Hashes a password using PBKDF2 with HMAC-SHA512.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A byte array containing the salt and the hashed password.</returns>
    public static byte[] HashPassword(string password)
    {
        byte[] salt = new byte[SaltSize];
        RandomNumberGenerator.Create().GetBytes(salt);

        var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName);
        byte[] hash = deriveBytes.GetBytes(KeySize);
        byte[] hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);
        return hashBytes;
    }

    /// <summary>
    /// Verifies a password against a given hash.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hashBytes">The hash to verify against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public static bool VerifyPassword(string password, byte[] hashBytes)
    {
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName);
        byte[] hash = deriveBytes.GetBytes(KeySize);

        for (int i = 0; i < KeySize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }
        return true;
    }
}
