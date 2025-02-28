using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Gateway.ApiClients;
using DuckpondExample.Gateway.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace DuckpondExample.Gateway.Controllers;
[Route("[controller]")]
[ApiController]
public class GatewayController : ControllerBase
{

    private readonly ILogger<GatewayController> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ApiGatewaySettings apiGatewaySettings;
    private readonly PermissionApiClient permissionApiClient;
    private readonly HttpClient httpClient;
    /// <summary>
    /// Initializes a new instance of the <see cref="GatewayController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="httpClient">The HTTP client.</param>
    public GatewayController(ILogger<GatewayController> logger, IHttpClientFactory httpClientFactory, ApiGatewaySettings apiGatewaySettings, PermissionApiClient permissionApiClient)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.apiGatewaySettings = apiGatewaySettings;
        this.permissionApiClient = permissionApiClient;
        httpClient = this.httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromMinutes(3);
    }

    /// <summary>
    /// Gets the API description for a specific service.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The API description as a JSON string.</returns>
    [HttpGet("/openid/{serviceName}/openapi.json")]
    public async Task<IActionResult> GetApiDescription(string serviceName)
    {
        (var key, var knownService) = apiGatewaySettings.ApiServices.FirstOrDefault(apiService =>
        {
            return apiService.Value.Name == serviceName;
        });

        if (knownService == null)
        {
            return NotFound();
        }

        try
        {
            var openApiJsonString = await httpClient.GetStringAsync($"{knownService.BaseUrl}{knownService.SwaggerPath}");
            var openApi = JsonConvert.DeserializeObject<JObject>(openApiJsonString);
            var pathsToken = openApi?.GetValue("paths");
            if (pathsToken == null)
            {
                return BadRequest("Paths not found in the OpenAPI description.");
            }
            var paths = pathsToken.Children<JToken>();

            var toRemove = new List<JProperty>();
            for (var i = 0; i < paths.Count(); ++i)
            {
                var property = paths.ElementAt(i).ToObject<JProperty>();
                var gatewayPath = $"/gateway{property.Name}";
                var existingValue = property.Value;
                var newProperty = new JProperty(gatewayPath, existingValue);
                paths.ElementAt(i).Replace(newProperty);
                if (!property.Name.Contains(key))
                {
                    toRemove.Add(newProperty);
                }
            }
            toRemove.ForEach(p => p.Remove());

            return Ok(openApi?.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Forwards a GET request to a microservice.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The response from the microservice.</returns>
    [HttpGet]
    [Route("{**catchAll}")]
    public async Task<IActionResult> GetMethodOnMicroservice()
    {
        return await ForwardCall((apiUrl, content) => httpClient.GetAsync(apiUrl));
    }

    /// <summary>
    /// Forwards a POST request to a microservice.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The response from the microservice.</returns>
    [HttpPost]
    [Route("{**catchAll}")]
    public async Task<IActionResult> PostMethodOnMicroservice()
    {
        return await ForwardCall((apiUrl, content) => httpClient.PostAsync(apiUrl, content));
    }

    /// <summary>
    /// Forwards a PUT request to a microservice.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The response from the microservice.</returns>
    [HttpPut]
    [Route("{**catchAll}")]
    public async Task<IActionResult> PutMethodOnMicroservice()
    {

        return await ForwardCall((apiUrl, content) => httpClient.PutAsync(apiUrl, content));
    }

    /// <summary>
    /// Forwards a PATCH request to a microservice.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The response from the microservice.</returns>
    [HttpPatch]
    [Route("{**catchAll}")]
    public async Task<IActionResult> PatchMethodOnMicroservice()
    {
        return await ForwardCall((apiUrl, content) => httpClient.PatchAsync(apiUrl, content));
    }

    /// <summary>
    /// Forwards a DELETE request to a microservice.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <returns>The response from the microservice.</returns>
    [HttpDelete]
    [Route("{**catchAll}")]
    public async Task<IActionResult> DeleteMethodOnMicroservice()
    {
        return await ForwardCall((apiUrl, content) => httpClient.DeleteAsync(apiUrl));
    }

    /// <summary>
    /// Forwards the call to the appropriate microservice based on the request path and method.
    /// </summary>
    /// <param name="action">The action to perform on the microservice (GET, POST, PUT, PATCH, DELETE).</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the forwarded call.</returns>
    private async Task<IActionResult> ForwardCall(Func<string, StringContent, Task<HttpResponseMessage>> action)
    {
        try
        {
            var apiPath = HttpContext.Request.Path.Value?.Replace("/gateway", "");
            var targetService = apiGatewaySettings.ApiServices.FirstOrDefault(s => apiPath.Contains(s.Key));
            var apiUrl = targetService.Value.BaseUrl + apiPath;
            var query = HttpContext.Request.QueryString.Value ?? string.Empty;
            var endpointSettings = targetService.Value.ExposedEndpoints.FirstOrDefault(e => apiPath.Contains(e.Path));

            if (endpointSettings == null)
            {
                return NotFound($"Service '{apiPath}' not found.");
            }

            // Check if the request must be authenticated
            string token = string.Empty;
            if (HttpContext.Request.Headers.Any(h => h.Key == "Authorization"))
            {
                token = HttpContext.Request.Headers.Single(h => h.Key == "Authorization").Value.ToString().Replace("Bearer ", "");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (endpointSettings.MustAuthenticate && string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            if (endpointSettings.MustAuthenticate && !string.IsNullOrEmpty(endpointSettings.Permission))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var isAdmin = Convert.ToBoolean(jwtSecurityToken.Payload.GetValueOrDefault("IsAdmin") ?? false);
                if (!isAdmin)
                {
                    var userId = jwtSecurityToken.Payload["Id"].ToString();
                    var hasPermissionRequestResult = await permissionApiClient.HasPermission(new PermissionRequestCommand { UserID = Convert.ToInt32(userId), Permission = endpointSettings.Permission });
                    if (!hasPermissionRequestResult.IsSuccess)
                    {
                        return Unauthorized();
                    }
                }
            }

            // Forward the request to the target service
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var content = new StringContent(await reader.ReadToEndAsync(), Encoding.UTF8, "application/json");
                var result = await action(apiUrl + query, content);

                var resultString = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode)
                {
                    return BadRequest(resultString);
                }
                return Ok(resultString);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
