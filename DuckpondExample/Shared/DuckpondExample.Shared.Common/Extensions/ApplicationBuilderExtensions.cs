using DuckpondExample.Shared.Common.Attributes;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace DuckpondExample.Shared.Common.Extensions;
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds services to the WebAssemblyHostBuilder based on the <see cref="AddAsServiceAttribute"/> attribute.
    /// </summary>
    /// <param name="builder">The <see cref="WebAssemblyHostBuilder"/> to add services to.</param>
    /// <returns>The <see cref="WebAssemblyHostBuilder"/> with the added services.</returns>
    public static WebAssemblyHostBuilder AddAttributedServices(this WebAssemblyHostBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        assemblies.SelectMany(asm => asm.GetTypes())
            .ForEach(t =>
            {
                var attr = t.GetCustomAttribute<AddAsServiceAttribute>();
                if (attr == null)
                {
                    return;
                }

                builder.Services.Add(new ServiceDescriptor(serviceType: attr.InterfaceType == null ? t : attr.InterfaceType, implementationType: t, lifetime: attr.Lifetime));
            });

        return builder;
    }
}
