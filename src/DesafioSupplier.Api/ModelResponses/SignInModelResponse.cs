using DesafioSupplier.Api.Shared;

namespace DesafioSupplier.Api.ModelResponses;

public class SignInModelResponse : IStatusResponse
{
    public string Status => "OK";
    public required string Token { get; set; }
}
