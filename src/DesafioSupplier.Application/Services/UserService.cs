using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Application.Auth;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Application.Services;

public class UserService(IUserRepository userRepository, PasswordHasher passwordHasher) : IUserService
{
    public async Task<string> SaveUserAsync(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        user.Password = passwordHasher.HashPassword(user.Password);
        await userRepository.SaveUserAsync(user);

        return user.Id;
    }

    public async Task<User> GetUserAsync(string email)
    {
        var user = await userRepository.GetUserAsync(email);

        return user;
    }
}
