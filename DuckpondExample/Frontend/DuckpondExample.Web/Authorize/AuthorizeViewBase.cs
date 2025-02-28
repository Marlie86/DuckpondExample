using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DuckpondExample.Web.Authorize;

/// <summary>
/// A base component for handling authorization views.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="permissionService">The permission service instance.</param>
public class AuthorizeViewBase(ILogger<AuthorizeViewBase> logger, PermissionService permissionService) : ComponentBase
{
    /// <summary>
    /// Gets or sets the content to display while loading.
    /// </summary>
    [Parameter] public RenderFragment LoadingContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the user is authorized.
    /// </summary>
    [Parameter] public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the user is denied permission.
    /// </summary>
    [Parameter] public RenderFragment PermissionDeniedContent { get; set; }

    /// <summary>
    /// Gets or sets the required permission for the view.
    /// </summary>
    [Parameter] public string Permission { get; set; }

    private AuthorizeViewBaseStateEnum currentState = AuthorizeViewBaseStateEnum.Loading;

    /// <summary>
    /// Initializes the component and checks the user's permission.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var isAuthorized = await permissionService.HasPermissionAsync(Permission);
        currentState = isAuthorized ? AuthorizeViewBaseStateEnum.Authorized : AuthorizeViewBaseStateEnum.PermissionDenied;
    }

    /// <summary>
    /// Builds the render tree for the component based on the current state.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        switch (currentState)
        {
            case AuthorizeViewBaseStateEnum.Loading:
                builder.AddContent(0, LoadingContent);
                break;
            case AuthorizeViewBaseStateEnum.Authorized:
                builder.AddContent(1, ChildContent);
                break;
            case AuthorizeViewBaseStateEnum.PermissionDenied:
                builder.AddContent(2, PermissionDeniedContent);
                break;
            default:
                throw new InvalidOperationException("Invalid state.");
        }
    }
}

/// <summary>
/// Represents the different states of the <see cref="AuthorizeViewBase"/> component.
/// </summary>
public enum AuthorizeViewBaseStateEnum
{
    Loading,
    Authorized,
    PermissionDenied
}
