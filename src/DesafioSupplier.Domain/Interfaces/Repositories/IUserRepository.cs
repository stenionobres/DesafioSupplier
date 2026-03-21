using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task SaveUserAsync(User user);
    Task<User> GetUserAsync(string email);
}
