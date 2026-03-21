namespace DesafioSupplier.Domain.Entities;

public class Customer
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Cpf { get; set; }
    public required decimal LimitValue { get; set; }
}
