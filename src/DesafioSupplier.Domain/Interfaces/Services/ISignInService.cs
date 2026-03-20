
namespace DesafioSupplier.Domain.Interfaces.Services;

public interface ISignInService
{
    Task<string> SignIn(string email, string password);
}
