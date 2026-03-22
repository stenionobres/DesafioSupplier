using DesafioSupplier.Domain.Entities;
using DesafioSupplier.ServicesAsync.Publishers;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Application.Services;

public class TransactionService(ITransactionRepository transactionRepository, TransactionPublisher transactionPublisher) : ITransactionService
{
    public async Task<string> PerformTransactionAsync(Transaction transaction)
    {
        transaction.Id = Guid.NewGuid().ToString();
        await transactionRepository.SaveTransactionAsync(transaction);
        await transactionPublisher.PublishAsync(new { transaction.CustomerId, transaction.Amount });

        return transaction.Id;
    }
}
