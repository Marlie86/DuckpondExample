namespace DuckpondExample.Gateway.Models
{
    /// <summary>
    /// Represents an endpoint that is exposed by the gateway.
    /// </summary>
    public class ExposedEndpoint
    {
        /// <summary>
        /// Gets or sets the name of the endpoint.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the endpoint.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method used by the endpoint.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether authentication is required for the endpoint.
        /// </summary>
        public bool MustAuthenticate { get; set; }

        /// <summary>
        /// Gets or sets the permission required to access the endpoint.
        /// </summary>
        public string Permission { get; set; }
    }
}
