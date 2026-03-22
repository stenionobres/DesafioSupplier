using DesafioSupplier.Application.Auth;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class SignInService(IUserService userService, PasswordHasher passwordHasher, ITokenService tokenService) : ISignInService
{
    public async Task<string> SignIn(string email, string password)
    {
        var user = await userService.GetUserAsync(email);

        if (user == null)
            throw new ApplicationException("Usuário não encontrado");

        if (passwordHasher.VerifyPassword(password, user.Password))
        {
            var token = await tokenService.GetTokenAsync(user);

            return token;
        }

        throw new ApplicationException("Senha inválida");
    }
}
