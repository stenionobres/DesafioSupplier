using Dapper;
using System.Data;

namespace DesafioSupplier.Persistence.Configuration;

public static class SqLiteDbInitializer
{
    public static void Initialize(IDbConnection db)
    {
        db.Execute(@"
            CREATE TABLE Users (
                Id TEXT PRIMARY KEY NOT NULL,
                Email TEXT NOT NULL,
                Password TEXT NOT NULL
            );
        ");

        db.Execute(@"
            CREATE TABLE Customers (
                Id TEXT PRIMARY KEY NOT NULL,
                Name TEXT NOT NULL,
                Cpf TEXT NOT NULL,
                LimitValue REAL NOT NULL
            );
        ");

        db.Execute(@"
            CREATE TABLE Transactions (
                Id TEXT PRIMARY KEY NOT NULL,
                CustomerId TEXT NOT NULL,
                Amount REAL NOT NULL
            );
        ");
    }
}
