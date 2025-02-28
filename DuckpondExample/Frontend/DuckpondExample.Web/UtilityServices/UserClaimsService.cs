using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.UtilityServices;

namespace Duckpond.Aspire.Web.UtilityServices;

[AddAsService(ServiceLifetime.Scoped)]
public class UserClaimsService
{
    private readonly ILogger<UserClaimsService> logger;
    private readonly StorageService storageService;

    public Dictionary<string, string> Claims { get; private set; } = new Dictionary<string, string>();

    public UserClaimsService(ILogger<UserClaimsService> logger, StorageService storageService)
    {
        this.logger = logger;
        this.storageService = storageService;
    }

    public async Task InitalizeAsync()
    {
        if (Claims.Count == 0)
        {
            Claims = await storageService.GetItemAsync<Dictionary<string, string>>("UserClaims") ?? new Dictionary<string, string>();
        }
    }

    public async Task<TResult?> GetClaim<TResult>(string key) where TResult : class
    {
        await InitalizeAsync();
        if (Claims.ContainsKey(key))
        {
            switch (typeof(TResult))
            {
                case Type t when t == typeof(string):
                    return Claims[key] as TResult;
                case Type t when t == typeof(int):
                    return int.TryParse(Claims[key], out var intResult) ? intResult as TResult : null;
                case Type t when t == typeof(bool):
                    return bool.TryParse(Claims[key], out var boolResult) ? boolResult as TResult : null;
                case Type t when t == typeof(DateTime):
                    return DateTime.TryParse(Claims[key], out var dateTimeResult) ? dateTimeResult as TResult : null;
                case Type t when t == typeof(float):
                    return float.TryParse(Claims[key], out var floatResult) ? floatResult as TResult : null;
                case Type t when t == typeof(double):
                    return double.TryParse(Claims[key], out var doubleResult) ? doubleResult as TResult : null;
                default: 
                    return Claims[key] as TResult;
            }
            
        }
        return null;
    }
}
