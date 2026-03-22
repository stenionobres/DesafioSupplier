namespace DesafioSupplier.Domain.Entities;

public class Transaction
{
    public required string Id { get; set; }
    public required string CustomerId { get; set; }
    public required decimal Amount { get; set; }
}
