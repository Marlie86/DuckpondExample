using Microsoft.Extensions.DependencyInjection;

namespace DuckpondExample.Shared.Common.Attributes;

/// <summary>
/// Attribute to specify that a class or interface should be added to the service collection.
/// </summary>
/// <remarks>
/// This attribute can be used to automatically register a class or interface with the specified service lifetime.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class AddAsServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    public Type? InterfaceType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAsServiceAttribute"/> class.
    /// </summary>
    /// <param name="lifetime">The service lifetime.</param>
    /// <param name="interfaceType">The interface type to register. If null, the class itself will be registered.</param>
    public AddAsServiceAttribute(ServiceLifetime lifetime, Type? interfaceType = null)
    {
        Lifetime = lifetime;
        InterfaceType = interfaceType;
    }
}
