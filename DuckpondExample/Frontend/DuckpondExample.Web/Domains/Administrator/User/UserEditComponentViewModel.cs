using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.Validators;

namespace DuckpondExample.Web.Domains.Administrator.User;

/// <summary>
/// ViewModel for editing a user's details in the administrator section.
/// </summary>
/// <remarks>
/// This ViewModel handles the validation and updating of a user's details.
/// It uses the <see cref="UserValidator"/> to validate the user's information.
/// </remarks>
[AddAsService(ServiceLifetime.Transient)]
public class UserEditComponentViewModel : BaseViewModel
{
    private ILogger<UserGroupsComponentViewModel> logger;

    /// <summary>
    /// Gets or sets the user model.
    /// </summary>
    public UserModel User { get; set; }

    /// <summary>
    /// Gets the validator for the user model.
    /// </summary>
    public UserValidator Validator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEditComponentViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="validator">The validator for validating the user model.</param>
    public UserEditComponentViewModel(ILogger<UserGroupsComponentViewModel> logger, UserValidator validator)
    {
        this.logger = logger;
        Validator = validator;
    }
}
