using Dapper;
using System.Data;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Persistence.Repositories;

public class CustomerRepository(IDbConnection dbConnection) : ICustomerRepository
{
    public async Task SaveCustomerAsync(Customer customer)
    {
        var sql = "INSERT INTO Customers (Id, Name, Cpf, LimitValue) VALUES (@Id, @Name, @Cpf, @LimitValue)";

        await dbConnection.ExecuteAsync(sql, customer);
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var sql = "SELECT Id, Name, Cpf, LimitValue FROM Customers";
        var customersResult = await dbConnection.QueryAsync<Customer>(sql);

        return customersResult.ToList();
    }
}
