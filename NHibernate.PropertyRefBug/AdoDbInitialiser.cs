using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace NHibernate.PropertyRefBug;

public static class AdoDbInitialiser
{
    const string SchemaSql = @"
CREATE TABLE TheOrder (
    Id INTEGER NOT NULL PRIMARY KEY,
    UniqueId TEXT NOT NULL,
    CreatedDate TEXT NOT NULL
);

CREATE TABLE LineItem (
    Id INTEGER NOT NULL PRIMARY KEY,
    OrderId TEXT NOT NULL,
    ItemName TEXT NOT NULL,
    Amount REAL NOT NULL
);";

    const string DataSql = @"
INSERT INTO TheOrder (Id, UniqueId, CreatedDate)
VALUES (1, '0ab92479-8a17-4dbc-9bef-ce4344940cec', '2024-09-19T12:10:00Z');
INSERT INTO LineItem(Id, OrderId, ItemName, Amount)
VALUES(1, '0ab92479-8a17-4dbc-9bef-ce4344940cec', 'Bananas', 5);";
    
    public static void CreateSchema(IDbConnection connection)
    {
        using var tran = connection.BeginTransaction();
        using var comm = connection.CreateCommand();
        comm.Transaction = tran;
        comm.CommandText = SchemaSql;
        comm.ExecuteNonQuery();
        tran.Commit();
    }
    
    public static void AddData(IDbConnection connection)
    {
        using var tran = connection.BeginTransaction();
        using var comm = connection.CreateCommand();
        comm.Transaction = tran;
        comm.CommandText = DataSql;
        comm.ExecuteNonQuery();
        tran.Commit();
    }

    public static DbConnection GetConnection() => new SQLiteConnection("Data Source=:memory:");
}