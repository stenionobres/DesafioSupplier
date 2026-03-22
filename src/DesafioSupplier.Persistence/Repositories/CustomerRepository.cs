using Dapper;
using System.Data;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Persistence.Repositories;

public class CustomerRepository(IDbConnection dbConnection) : ICustomerRepository
{
    public async Task<Customer> GetCustomerByCpfAsync(string cpf)
    {
        var sql = "SELECT Id, Name, Cpf, LimitValue FROM Customers WHERE Cpf = @Cpf";
        var customersResult = await dbConnection.QueryAsync<Customer>(sql, new { Cpf = cpf });

        return customersResult.FirstOrDefault()!;
    }

    public async Task<Customer> GetCustomerByIdAsync(string customerId)
    {
        var sql = "SELECT Id, Name, Cpf, LimitValue FROM Customers WHERE Id = @Id";
        var customersResult = await dbConnection.QueryAsync<Customer>(sql, new { Id = customerId });

        return customersResult.FirstOrDefault()!;
    }

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
