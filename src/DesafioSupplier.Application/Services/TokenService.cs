using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class TokenService : ITokenService
{
    public Task<string> GetTokenAsync(User user)
    {
        return Task.FromResult("ExemploToken");
    }
}
