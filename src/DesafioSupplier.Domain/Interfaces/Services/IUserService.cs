using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Services;

public interface IUserService
{
    Task<string> SaveUserAsync(User user);
    Task<User> GetUserAsync(string email);
}
