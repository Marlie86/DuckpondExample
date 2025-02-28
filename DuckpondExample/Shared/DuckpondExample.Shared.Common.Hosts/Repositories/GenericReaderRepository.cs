using Dapper;

using DuckpondExample.Shared.Common.Extensions;

using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.Serialization;

namespace DuckpondExample.Shared.Common.Hosts.Repositories;

/// <summary>
/// Represents a generic repository for reading entities from a database using Dapper.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class GenericReaderRepository<T> : IGenericReaderRepository<T> where T : class
{
    protected readonly ILogger<T> logger;
    protected readonly DapperContext _context;
    protected readonly string _tableName;
    protected readonly List<string> _columnNames;
    protected readonly string _idColumnName = "Id";
    protected readonly PropertyInfo? _keyProperty;
    protected readonly List<string> _membersToIgnore = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericReaderRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The Dapper context.</param>
    /// <param name="tableName">The name of the table. If not provided, the table name is derived from the entity type.</param>
    public GenericReaderRepository(ILogger<T> logger, DapperContext context, string tableName = "")
    {
        this.logger = logger;
        _context = context;
        var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
        if (tableAttribute == null)
        {
            _tableName = typeof(T).Name + "s";
        }
        else
        {
            _tableName = tableAttribute.Name;
        }

        _keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());
        if (_keyProperty != null)
        {
            _idColumnName = _keyProperty.Name;
        }

        typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), true).Any()).ForEach(p => _membersToIgnore.Add(p.Name), (ex, msg) => logger.LogError(ex, msg));

        _columnNames = typeof(T).GetProperties().Select(p => p.Name).ToList();
    }

    /// <summary>
    /// Executes a query asynchronously and returns a collection of entities.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">The parameters for the query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<IEnumerable<T>> ExecuteQuery(string query, object? parameters = null)
    {
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<T>(query, parameters);
            return result;
        }
    }

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_tableName}";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<T>(query);
        }
    }
}
