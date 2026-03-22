using Dapper;
using System.Data;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Persistence.Repositories;

public class TransactionRepository(IDbConnection dbConnection) : ITransactionRepository
{
    public async Task SaveTransactionAsync(Transaction transaction)
    {
        var sql = "INSERT INTO Transactions (Id, CustomerId, Amount) VALUES (@Id, @CustomerId, @Amount)";

        await dbConnection.ExecuteAsync(sql, transaction);
    }
}
