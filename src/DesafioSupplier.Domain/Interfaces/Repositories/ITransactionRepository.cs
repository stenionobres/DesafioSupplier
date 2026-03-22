using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task SaveTransactionAsync(Transaction transaction);
}
