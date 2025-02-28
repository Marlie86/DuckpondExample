using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Web.ApiClients;

using FluentValidation;

namespace DuckpondExample.Web.Validators;

/// <summary>
/// Validator for the <see cref="UserModel"/> class.
/// </summary>
public class UserValidator : AbstractValidator<UserModel>
{
    private readonly UserApiClient userApiClient;

    private IEnumerable<UserModel> users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserValidator"/> class.
    /// Sets up validation rules for the <see cref="UserModel"/> properties.
    /// </summary>
    public UserValidator(UserApiClient userApiClient)
    {
        this.userApiClient = userApiClient;
        RuleFor(x => x.Username).NotEmpty().Length(3, 64).CustomAsync(async (s, m, c) =>
        {
            var username = m.InstanceToValidate.Username ?? string.Empty;
            if (username.Equals("NO LOGON"))
            {
                return;
            }
            var userId = m.InstanceToValidate.UserID;
            if (users == null)
            {
                var result = await this.userApiClient.GetUsers(c);
                users = result.Users;
            }
            var hasDuplicate = users.Any(x => x.Username == username && x.UserID != userId);
            if (hasDuplicate)
                m.AddFailure("Username", "Username must be unique.");
        });
        RuleFor(x => x.EMailAddress).NotEmpty().EmailAddress().CustomAsync(async (s, m, c) =>
        {
            var userId = m.InstanceToValidate.UserID;
            if (users == null)
            {
                var result = await this.userApiClient.GetUsers(c);
                users = result.Users;
            }
            var hasDuplicate = users.Any(x => x.EMailAddress == m.InstanceToValidate.EMailAddress && x.UserID != userId);
            if (hasDuplicate)
                m.AddFailure("Username", "Username must be unique.");
        });
        RuleFor(x => x.Password).CustomAsync(async (s, m, c) =>
        {
            var password = m.InstanceToValidate.Password ?? string.Empty;
            if (string.IsNullOrWhiteSpace(password))
                return;

            if (password.Length < 6)
                m.AddFailure("Password", "Password must be at least 8 characters long.");
            //var hasNumber = password.Any(char.IsDigit);
            //if (!hasNumber)
            //    m.AddFailure("Password", "Password must contain at least one number.");
            //var hasUpper = password.Any(char.IsUpper);
            //if (!hasUpper)
            //    m.AddFailure("Password", "Password must contain at least one uppercase letter.");
            //var hasLower = password.Any(char.IsLower);
            //if (!hasLower)
            //    m.AddFailure("Password", "Password must contain at least one lowercase letter.");
            //var hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));
            //if (!hasSpecial)
            //    m.AddFailure("Password", "Password must contain at least one special character.");
        });
    }

    /// <summary>
    /// Asynchronously validates a specific property of the <see cref="UserModel"/>.
    /// </summary>
    /// <param name="model">The model to validate.</param>
    /// <param name="propertyName">The name of the property to validate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of validation error messages.</returns>
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UserModel>.CreateWithOptions((UserModel)model, x => x.IncludeProperties(propertyName.Split(".")[1])));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
