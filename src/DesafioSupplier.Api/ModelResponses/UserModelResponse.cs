using DesafioSupplier.Api.Shared;

namespace DesafioSupplier.Api.ModelResponses;

public class UserModelResponse : IStatusResponse
{
    public string Status => "OK";
    public required string Id { get; set; }
}
