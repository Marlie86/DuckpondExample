namespace DuckpondExample.Gateway.Models
{
    /// <summary>
    /// Represents the settings for the API Gateway.
    /// </summary>
    public class ApiGatewaySettings
    {
        /// <summary>
        /// Gets or sets the dictionary of API services.
        /// The key is the service name and the value is the <see cref="ApiService"/> object.
        /// </summary>
        public Dictionary<string, ApiService> ApiServices { get; set; }
    }
}
