using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<string> SaveUserAsync(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await userRepository.SaveUserAsync(user);

        return user.Id;
    }

    public Task<User> GetUserAsync(string email)
    {
        var user = userRepository.GetUserAsync(email);
        
        return user;
    }
}
