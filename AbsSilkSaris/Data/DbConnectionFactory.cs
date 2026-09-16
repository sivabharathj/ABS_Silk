using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace AbsSilkSaris.Data;

public interface IDbConnectionFactory
{
    IDbConnection Create();
    bool IsSqlServer { get; }
    string LastInsertIdSql { get; }
}

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public bool IsSqlServer { get; }
    public string LastInsertIdSql => IsSqlServer
        ? "SELECT CAST(SCOPE_IDENTITY() AS INT);"
        : "SELECT last_insert_rowid();";

    public DbConnectionFactory(IConfiguration config)
    {
        IsSqlServer = string.Equals(config["Database:Provider"], "SqlServer", StringComparison.OrdinalIgnoreCase);
        _connectionString = IsSqlServer
            ? config.GetConnectionString("SqlServer") ?? config.GetConnectionString("DefaultConnection")!
            : config.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection Create() =>
        IsSqlServer ? new SqlConnection(_connectionString) : new SqliteConnection(_connectionString);
}
