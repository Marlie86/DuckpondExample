namespace DuckpondExample.Shared.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Executes the specified action on each element of the enumerable collection.
        /// If an exception occurs, the specified log action is called with the exception and a message.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable collection.</typeparam>
        /// <param name="self">The enumerable collection.</param>
        /// <param name="action">The action to execute on each element.</param>
        /// <param name="logAction">The action to log exceptions that occur during the execution of the action.</param>
        /// <returns>The original enumerable collection.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action, Action<Exception, string>? logAction = null)
        {
            foreach (var item in self)
            {
                try
                {
                    action(item);
                }
                catch (Exception ex)
                {
                    logAction?.Invoke(ex, "Error processing item in ForEach");
                }
            }

            return self;
        }

        /// <summary>
        /// Executes the specified action on each element of the enumerable collection.
        /// If an exception occurs, the specified log action is called with the exception and a message.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable collection.</typeparam>
        /// <param name="self">The enumerable collection.</param>
        /// <param name="action">The action to execute on each element.</param>
        /// <param name="logAction">The action to log exceptions that occur during the execution of the action.</param>
        /// <returns>The original enumerable collection.</returns>
        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> self, Func<T, Task> action, Action<Exception, string>? logAction = null)
        {
            self.AsParallel().ForAll(async item => await action(item));

            return self;
        }
    }
}
