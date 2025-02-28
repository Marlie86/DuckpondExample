using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

/// <summary>
/// Represents a repository for managing users entities.
/// </summary>
[AddAsService(ServiceLifetime.Singleton)]
public class UserRepository : GenericRepository<User>
{
    private readonly ILogger<User> logger;
    private readonly DapperContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="context">The Dapper context for database operations.</param>
    public UserRepository(ILogger<User> logger, DapperContext context) : base(logger, context)
    {
        this.logger = logger;
        this.context = context;
    }
}
