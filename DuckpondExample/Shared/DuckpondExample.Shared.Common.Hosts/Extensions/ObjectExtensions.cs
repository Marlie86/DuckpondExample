namespace DuckpondExample.Shared.Common.Hosts.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Copies properties from the source object to the destination object.
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TDest">The type of the destination object.</typeparam>
        /// <param name="source">The source object from which to copy properties.</param>
        /// <param name="destination">The destination object to which properties are copied.</param>
        /// <returns>The destination object with copied properties.</returns>
        public static TDest? CopyProperties<TSource, TDest>(this TSource? source, TDest? destination) where TSource : class where TDest : class
        {
            if (source == null || destination == null)
            {
                return destination;
            }
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
                if (destinationProperty != null)
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                }
            }
            return destination;
        }
    }
}
