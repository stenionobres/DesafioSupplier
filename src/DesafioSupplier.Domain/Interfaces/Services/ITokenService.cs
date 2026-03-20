using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<string> GetTokenAsync(User user);
}
