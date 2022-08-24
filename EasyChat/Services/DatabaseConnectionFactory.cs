using System.Data.Common;
using EasyChat.Interfaces;
using EasyChat.Models;
using Microsoft.Extensions.Options;
using Npgsql;

namespace EasyChat.Services;

public class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    private readonly DatabaseOptions _databaseOptions;

    public DatabaseConnectionFactory(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions.Value;
    }

    public DbConnection CreateConnection()
    {
        var conn = new NpgsqlConnection(_databaseOptions.ConnectionString);
        conn.Open();
        return conn;
    }

    public async Task<DbConnection> CreateConnectionAsync()
    {
        var conn = new NpgsqlConnection(_databaseOptions.ConnectionString);
        await conn.OpenAsync();
        return conn;
    }
}