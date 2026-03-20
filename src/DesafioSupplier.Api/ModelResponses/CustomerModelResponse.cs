using DesafioSupplier.Api.Shared;

namespace DesafioSupplier.Api.ModelResponses;

public class CustomerModelResponse : IStatusResponse
{
    public string Status => "OK";
    public required string IdCliente { get; set; }
}
