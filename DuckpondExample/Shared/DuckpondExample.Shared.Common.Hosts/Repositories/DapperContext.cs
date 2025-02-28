using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;

namespace DuckpondExample.Shared.Common.Hosts.Repositories;

/// <summary>
/// Represents a context for creating database connections.
/// </summary>
public class DapperContext
{
    private readonly IConfiguration _configuration;
    private string _connectionString;

    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    /// <returns>A new instance of <see cref="IDbConnection"/>.</returns>
    public virtual IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    /// <summary>
    /// Initializes a new instance of the <see cref="DapperContext"/> class.
    /// </summary>
    /// <param name="configuration">The configuration to use for retrieving the connection string.</param>
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new NullReferenceException("'DefaultConnection' connection string not set.");
        }
        _connectionString = connectionString;
    }
}
