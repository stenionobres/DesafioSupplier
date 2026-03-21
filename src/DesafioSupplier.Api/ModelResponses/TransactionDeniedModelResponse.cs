using DesafioSupplier.Api.Shared;

namespace DesafioSupplier.Api.ModelResponses;

public class TransactionDeniedModelResponse : IStatusResponse
{
    public string Status => "NEGADO";
}
