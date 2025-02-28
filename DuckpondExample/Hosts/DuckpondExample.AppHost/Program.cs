/// <summary>
/// The main entry point for the application.
/// </summary>
/// <param name="args">The command-line arguments.</param>
public class Program
{
    /// <summary>
    /// The main method that configures and runs the distributed application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var cache = builder.AddRedis("cache");

        var core = builder.AddProject<Projects.DuckpondExample_Services_Core_ServiceHost>("core");


        var gateway = builder.AddProject<Projects.DuckpondExample_Gateway>("gateway")
            .WithExternalHttpEndpoints()
            .WithReference(cache)
            .WaitFor(cache)
            .WithReference(core)
            .WaitFor(core);

        builder.AddProject<Projects.DuckpondExample_Web>("frontend")
            .WithExternalHttpEndpoints()
            .WithReference(cache)
            .WaitFor(cache)
            .WithReference(core)
            .WaitFor(core)
            .WithReference(gateway)
            .WaitFor(gateway);

        await builder.Build().RunAsync();
    }
}

