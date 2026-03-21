namespace DesafioSupplier.Api.ModelRequests;

public class TransactionModelRequest
{
    public required string IdCliente { get; set; }
    public required decimal ValorSimulacao { get; set; }
}
