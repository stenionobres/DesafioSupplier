using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class UserService : IUserService
{
    public Task<string> SaveUserAsync(User user)
    {
        return Task.FromResult("");
    }
}
