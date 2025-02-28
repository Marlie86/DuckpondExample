using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a logon attempt.
/// </summary>
public class LogonResult : BaseResult
{
    /// <summary>
    /// Gets or sets the token data associated with the logon result.
    /// </summary>
    public TokenData? TokenData { get; set; } = null;
}
