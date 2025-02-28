namespace DuckpondExample.Shared.Common.BaseClasses
{
    /// <summary>
    /// Base class for all result models.
    /// </summary>
    public class BaseResult
    {
        /// <value>
        /// <c>true</c> if the operation was successful; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; } = false;

        /// <value>
        /// A list of error messages if the operation failed.
        /// </value>
        public List<string> ErrorMessages { get; set; } = [];
    }
}
