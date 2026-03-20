using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class SignInService(IUserService userService, ITokenService tokenService) : ISignInService
{
    public async Task<string> SignIn(string email, string password)
    {
        var user = await userService.GetUserAsync(email);
        var token = await tokenService.GetTokenAsync(user);

        return token;
    }
}
