using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Services;

public interface ITransactionService
{
    Task<string> PerformTransactionAsync(Transaction transaction);
}
