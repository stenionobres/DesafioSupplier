using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class TransactionService : ITransactionService
{
    public Task<string> PerformTransaction(string customerId, decimal amount)
    {
        return Task.FromResult("TransactionOk");
    }
}
