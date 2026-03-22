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
        
        db.Execute(@"
            INSERT INTO Users (Id, Email, Password) 
            VALUES ('0952ee8d-69f1-4a85-8572-faf5856fde4e', 'email@example.com', 'JGGuYAhZ0y9FryuJdfkRWnaRNP+Xv1vDlrTQ2SJzYJnwtlWSXSNHW6M5bMFowGSW')
        ");

        db.Execute(@"
            INSERT INTO Customers (Id, Name, Cpf, LimitValue) 
            VALUES ('d61ead45-a8f6-4dd9-8ecf-ca5acda0abfc', 'Joao da Silva', '74709439001', 20)
        ");
    }
}
