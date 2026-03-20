namespace DesafioSupplier.Domain.Entities;

public class Customer
{
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
    public required decimal ValorLimite { get; set; }
}
