using DesafioSupplier.ServicesAsync.Publishers;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class TransactionService(TransactionPublisher transactionPublisher) : ITransactionService
{
    public async Task<string> PerformTransaction(string customerId, decimal amount)
    {
        await transactionPublisher.PublishAsync(new { customerId, amount });

        return "TransactionOk";
    }
}
