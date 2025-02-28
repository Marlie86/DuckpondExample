namespace DuckpondExample.Shared.Common.Hosts.Repositories
{
    public interface IGenericReaderRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Executes a query asynchronously.
        /// </summary>
        /// <param name="query">The query string to execute.</param>
        /// <param name="parameters">The parameters to pass to the query, if any.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>

        Task<IEnumerable<T>> ExecuteQuery(string query, object? parameters = null);
    }
}
