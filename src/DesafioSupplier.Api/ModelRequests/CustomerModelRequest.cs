namespace DesafioSupplier.Api.ModelRequests;

public class CustomerModelRequest
{
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
    public required decimal ValorLimite { get; set; }
}
