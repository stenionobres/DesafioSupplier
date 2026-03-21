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
    }
}
