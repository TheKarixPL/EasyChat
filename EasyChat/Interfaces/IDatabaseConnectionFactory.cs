using System.Data.Common;

namespace EasyChat.Interfaces;

public interface IDatabaseConnectionFactory
{
    DbConnection CreateConnection();
    Task<DbConnection> CreateConnectionAsync();
}