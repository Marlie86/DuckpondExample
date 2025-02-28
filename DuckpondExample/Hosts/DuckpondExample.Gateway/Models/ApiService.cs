namespace DuckpondExample.Gateway.Models
{
    /// <summary>
    /// Represents an API service configuration.
    /// </summary>
    public class ApiService
    {
        /// <summary>
        /// Gets or sets the name of the API service.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the base URL of the API service.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the path to the Swagger documentation.
        /// </summary>
        public string SwaggerPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the Swagger documentation.
        /// </summary>
        public string SwaggerName { get; set; }

        /// <summary>
        /// Gets or sets the title of the Swagger documentation.
        /// </summary>
        public string SwaggerTitle { get; set; }

        /// <summary>
        /// Gets or sets the version of the Swagger documentation.
        /// </summary>
        public string SwaggerVersion { get; set; }

        /// <summary>
        /// Gets or sets the description of the Swagger documentation.
        /// </summary>
        public string SwaggerDescription { get; set; }

        /// <summary>
        /// Gets or sets the list of exposed endpoints for the API service.
        /// </summary>
        public List<ExposedEndpoint> ExposedEndpoints { get; set; }
    }
}
