using System.Security.Cryptography;

namespace DesafioSupplier.Application.Auth;

//hash with salt
public class PasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int KeySize = 32;  // 256 bits
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA256);

        var salt = algorithm.Salt;
        var key = algorithm.GetBytes(KeySize);

        return Convert.ToBase64String(
            Combine(salt, key));
    }

    public bool VerifyPassword(string password, string hash)
    {
        var bytes = Convert.FromBase64String(hash);

        var salt = new byte[SaltSize];
        var key = new byte[KeySize];

        Array.Copy(bytes, 0, salt, 0, SaltSize);
        Array.Copy(bytes, SaltSize, key, 0, KeySize);

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        var keyToCheck = algorithm.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }

    private byte[] Combine(byte[] a, byte[] b)
    {
        var result = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, result, 0, a.Length);
        Buffer.BlockCopy(b, 0, result, a.Length, b.Length);
        return result;
    }
}
