namespace DuckpondExample.Services.Core.Shared.Models
{
    /// <summary>
    /// Represents the data associated with an authentication token.
    /// </summary>
    public class TokenData
    {
        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration date and time of the token.
        /// </summary>
        public DateTime Expiration { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the refresh token used to obtain a new authentication token.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
