namespace DesafioSupplier.Domain.Interfaces.Services;

public interface ITransactionService
{
    Task<string> PerformTransaction(string customerId, decimal amount);
}
