using Dapper;

using DuckpondExample.Shared.Common.Extensions;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.Serialization;

namespace DuckpondExample.Shared.Common.Hosts.Repositories;


/// <summary>
/// Generic repository for CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ILogger<T> logger;
    protected readonly DapperContext _context;
    protected readonly string _tableName;
    protected readonly List<string> _columnNames;
    protected readonly string _idColumnName = "Id";
    protected readonly PropertyInfo? _keyProperty;
    protected readonly List<string> _membersToIgnore = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="context">The Dapper context.</param>
    public GenericRepository(ILogger<T> logger, DapperContext context)
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
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>True if the entity was deleted; otherwise, false.</returns>
    public bool Delete(int id)
    {
        try
        {
            logger.LogInformation($"Trying to delete entity with ID {id}.");
            var query = $"DELETE FROM {_tableName} WHERE {_idColumnName} = @Id";
            logger.LogInformation($"Query: {query}");
            using (var connection = _context.CreateConnection())
            {
                var result = connection.Execute(query, new { Id = id });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting entity.");
            return false;
        }
    }
    /// <summary>
    /// Deletes an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the entity was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            logger.LogInformation($"Trying to delete entity with ID {id}.");
            var query = $"DELETE FROM {_tableName} WHERE {_idColumnName} = @Id";
            logger.LogInformation($"Query: {query}");   
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting entity.");
            return false;
        }
    }

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    public IEnumerable<T> GetAll()
    {
        try
        {
            logger.LogInformation("Trying to get all entities.");
            var query = $"SELECT * FROM {_tableName}";
            logger.LogInformation($"Query: {query}");
            using (var connection = _context.CreateConnection())
            {
                return connection.Query<T>(query);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all entities.");
            return new List<T>();
        }
    }

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            logger.LogInformation("Trying to get all entities.");
            var query = $"SELECT * FROM {_tableName}";
            logger.LogInformation($"Query: {query}");
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<T>(query);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all entities.");
            return new List<T>();
        }
    }

    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public T? GetById(int id)
    {
        try
        {
            logger.LogInformation($"Trying to get entity with ID {id}.");
            var query = $"SELECT * FROM {_tableName} WHERE {_idColumnName} = @Id";
            logger.LogInformation($"Query: {query}");
            using (var connection = _context.CreateConnection())
            {
                return connection.QuerySingleOrDefault<T>(query, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting entity by id.");
            return null;
        }
    }

    /// <summary>
    /// Gets an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInformation($"Trying to get entity with ID {id}.");
            var query = $"SELECT * FROM {_tableName} WHERE {_idColumnName} = @Id";
            logger.LogInformation($"Query: {query}");
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting entity by id.");
            return null;
        }
    }

    /// <summary>
    /// Inserts a new entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The identifier of the inserted entity.</returns>
    public int Insert(T entity)
    {
        logger.LogInformation($"Trying to insert entity.");  
        var columns = _columnNames.Where(cn => !_membersToIgnore.Contains(cn) /*&&  _keyProperty?.Name != cn*/).ToArray();
        var query = $"INSERT INTO {_tableName}({string.Join(",", columns)}) VALUES (@{string.Join(", @", columns)});";
        logger.LogInformation($"Query: {query}");
        using (var connection = _context.CreateConnection())
        {
            try
            {
                connection.Execute($"SET IDENTITY_INSERT {_tableName} ON");
                var rowsAffected = connection.Execute(query, entity, null, null, null);
                if (rowsAffected == 1)
                {
                    query = $"SELECT MAX({_idColumnName}) FROM {_tableName}";
                    return connection.QuerySingle<int>(query);
                }
                return -1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting entity.");
                return -1;
            }
            finally
            {
                connection.Execute($"SET IDENTITY_INSERT {_tableName} OFF");
            }
        }
    }


    /// <summary>
    /// Inserts a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the inserted entity.</returns>
    public async Task<int> InsertAsync(T entity)
    {
        try
        {
            logger.LogInformation($"Trying to insert entity.");
            var columns = _columnNames.Where(cn => !_membersToIgnore.Contains(cn) && _keyProperty?.Name != cn).ToArray();
            var query = $"INSERT INTO {_tableName} ({string.Join(",", columns)}) VALUES (@{string.Join(", @", columns)});";
            logger.LogInformation($"Query: {query}");
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, entity);
                if (rowsAffected == 1)
                {
                    query = $"SELECT MAX({_idColumnName}) FROM {_tableName}";
                    return connection.QuerySingle<int>(query);
                }
                return -1;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting entity.");
            return -1;
        }
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public int Update(T entity)
    {
        try
        {
            logger.LogInformation("Trying to update entity.");
            var nullMembers = new List<string>();
            entity.GetType().GetProperties().Where(p => p.GetValue(entity) == null).ForEach(p => nullMembers.Add(p.Name), (ex, msg) => logger.LogError(ex, msg));

            var setValues = _columnNames.Where(cn => !_membersToIgnore.Contains(cn) && _keyProperty?.Name != cn && !nullMembers.Contains(cn)).Select(prop => $"{prop} = @{prop}");
            var query = $"UPDATE {_tableName} SET {string.Join(", ", setValues)} WHERE {_idColumnName} = @{_idColumnName}";
            logger.LogInformation($"Query: {query}");

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Execute(query, entity);
                return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating entity.");
            return -1;
        }
    }

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    public async Task<int> UpdateAsync(T entity)
    {
        try
        {
            logger.LogInformation("Trying to update entity.");  
            var nullMembers = new List<string>();
            entity.GetType().GetProperties().Where(p => p.GetValue(entity) == null).ForEach(p => nullMembers.Add(p.Name), (ex, msg) => logger.LogError(ex, msg));

            var setValues = _columnNames.Where(cn => !_membersToIgnore.Contains(cn) && _keyProperty?.Name != cn && !nullMembers.Contains(cn)).Select(prop => $"{prop} = @{prop}");
            var query = $"UPDATE {_tableName} SET {string.Join(", ", setValues)} WHERE {_idColumnName} = @{_idColumnName}";
            logger.LogInformation($"Query: {query}");

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, entity);
                return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating entity.");
            return -1;
        }
    }
}
