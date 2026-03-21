using DesafioSupplier.Api.Shared;

namespace DesafioSupplier.Api.ModelResponses;

public class TransactionModelResponse : IStatusResponse
{
    public string Status => "APROVADO";
    public required string IdTransacao { get; set; }
}
