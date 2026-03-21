using Dapper;
using System.Data;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Persistence;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    public async Task SaveUserAsync(User user)
    {
        var sql = "INSERT INTO Users (Id, Email, Password) VALUES (@Id, @Email, @Password)";

        await dbConnection.ExecuteAsync(sql, user);
    }

    public async Task<User> GetUserAsync(string email)
    {
        var sql = "SELECT Id, Email, Password FROM Users WHERE Email = @Email";
        var userResult = await dbConnection.QueryAsync<User>(sql, new { Email = email });

        return userResult.FirstOrDefault()!;
    }
}
