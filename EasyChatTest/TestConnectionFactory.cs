using System.Data.Common;
using EasyChat.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EasyChatTest;

public class TestConnectionFactory : IDatabaseConnectionFactory
{
    private readonly string _connectionString;
    
    public TestConnectionFactory()
    {
        var cb = new ConfigurationBuilder();
        cb.AddJsonFile("appsettings.Development.json").AddJsonFile("secret.Development.json");
        var config = cb.Build();
        _connectionString = config["Database:ConnectionString"];
    }
    
    public DbConnection CreateConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }

    public async Task<DbConnection> CreateConnectionAsync()
    {
        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        return conn;
    }
}