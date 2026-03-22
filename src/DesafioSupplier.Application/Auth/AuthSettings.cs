namespace DesafioSupplier.Application.Auth;

public class AuthSettings
{
    public required int TokenDurationInHours { get; set; }
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
}
