using System.Security.Cryptography;
using DesafioSupplier.Domain.Interfaces.Auth;

namespace DesafioSupplier.Application.Auth;

//hash with salt
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int KeySize = 32;  // 256 bits
    private const int Iterations = 10000;

    public Task<string> HashPasswordAsync(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA256);

        var salt = algorithm.Salt;
        var key = algorithm.GetBytes(KeySize);
        var hash = Convert.ToBase64String(Combine(salt, key));

        return Task.FromResult(hash);
    }

    public Task<bool> VerifyPasswordAsync(string password, string hash)
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
        var passwordIsValid = CryptographicOperations.FixedTimeEquals(key, keyToCheck);

        return Task.FromResult(passwordIsValid);
    }

    private byte[] Combine(byte[] a, byte[] b)
    {
        var result = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, result, 0, a.Length);
        Buffer.BlockCopy(b, 0, result, a.Length, b.Length);
        return result;
    }
}
