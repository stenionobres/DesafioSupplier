using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.Domain.Interfaces.Publisher;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Application.Services;

public class TransactionService(ICustomerRepository customerRepository, 
                                ITransactionRepository transactionRepository, 
                                IPublisher transactionPublisher) : ITransactionService
{
    public async Task<string> PerformTransactionAsync(Transaction transaction)
    {
        var savedCustomer = await customerRepository.GetCustomerByIdAsync(transaction.CustomerId);
        
        if (savedCustomer == null)
            throw new ApplicationException("Não existe cliente com esse Id");

        if (transaction.Amount > savedCustomer.LimitValue)
            throw new ApplicationException("Valor solicitado é superior ao limite do cliente");

        transaction.Id = Guid.NewGuid().ToString();
        
        await transactionRepository.SaveTransactionAsync(transaction);
        await transactionPublisher.PublishAsync(transaction);

        return transaction.Id;
    }
}
