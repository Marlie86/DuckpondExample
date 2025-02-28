using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Enums;
using DuckpondExample.Web.Models;

using MudBlazor;

using System.ComponentModel;

namespace DuckpondExample.Web.Domains.Administrator.Groups;

/// <summary>
/// ViewModel for managing groups in the administrator domain.
/// </summary>
[AddAsService(ServiceLifetime.Scoped)]
public class GroupsViewModel : BaseViewModel
{
    private ILogger<GroupsViewModel> logger;
    private Group selectedGroup;
    private readonly GroupsApiClient groupsApiClient;
    private readonly ISnackbar snackbar;
    private readonly IDialogService dialogService;

    /// <summary>
    /// Gets or sets the collection of groups.
    /// </summary>
    public IEnumerable<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets the selected group.
    /// </summary>
    /// <value>
    /// The selected group.
    /// </value>
    /// <remarks>
    /// When the selected group changes, the <see cref="RaisePropertyChanged"/> method is called to notify listeners.
    /// </remarks>
    public Group SelectedGroup
    {
        get => selectedGroup;
        set
        {
            if (selectedGroup == value) return;
            selectedGroup = value;
            RaisePropertyChanged();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupsViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="groupsApiClient">The API client for fetching groups.</param>
    public GroupsViewModel(ILogger<GroupsViewModel> logger, GroupsApiClient groupsApiClient, ISnackbar snackbar, IDialogService dialogService)
    {
        this.logger = logger;
        this.groupsApiClient = groupsApiClient;
        this.snackbar = snackbar;
        this.dialogService = dialogService;
    }

    /// <summary>
    /// Initializes the ViewModel asynchronously by fetching the groups.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override async Task InitializeAsync()
    {
        PropertyChanged += GroupsViewModel_PropertyChanged;
        var getGroupsResult = await groupsApiClient.GetGroupsAsync();
        if (!getGroupsResult.IsSuccess)
        {
            logger.LogWarning("Error fetching groups.");
        }
        Groups = getGroupsResult.Groups;
    }

    /// <summary>
    /// Opens a dialog for editing the specified group.
    /// </summary>
    /// <param name="group">The group to be edited.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenGroupEditDialog(Group group)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true };
        var dialogParameters = new DialogParameters()
        {
            { "Group", group },
            { "Mode", DialogModeEnum.Edit }
        };

        var dialog = await dialogService.ShowAsync<GroupEditDialog>("Gruppe bearbeiten", dialogParameters, options);
        var dialogResult = await dialog.Result;
        if (dialogResult == null || dialogResult.Canceled)
        {
            await InitializeAsync();
            return;
        }

        var resultData = dialogResult.Data as bool?;
        if (resultData == true)
        {
            await InitializeAsync();
            logger.LogInformation("User updated successfully.");
            return;
        }
        await InitializeAsync();
        logger.LogInformation("Error updating user.");
    }


    /// <summary>
    /// Opens a dialog for creating a new group.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenGroupCreateDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true };
        var dialogParameters = new DialogParameters()
        {
            { "Group", new Group() },
            { "Mode", DialogModeEnum.Create }
        };

        var dialog = await dialogService.ShowAsync<GroupEditDialog>("Gruppe erstellen", dialogParameters, options);
        var dialogResult = await dialog.Result;
        if (dialogResult == null || dialogResult.Canceled)
        {
            await InitializeAsync();
            return;
        }

        var resultData = dialogResult.Data as bool?;
        if (resultData == true)
        {
            await InitializeAsync();
            logger.LogInformation("User updated successfully.");
            return;
        }
        await InitializeAsync();
        logger.LogInformation("Error updating user.");
    }

    /// <summary>
    /// Creates a new group and sets it as the selected group.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateNewGroup()
    {
        SelectedGroup = new Group();
    }

    /// <summary>
    /// Removes the specified group after confirming with the user.
    /// </summary>
    /// <param name="groupToDelete">The group to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveGroup(Group groupToDelete)
    {
        var doDelete = await dialogService.ShowMessageBox("Gruppe löschen", "Möchten Sie die Gruppe wirklich löschen?", yesText: "Ja", cancelText: "Nein");
        if (!doDelete.GetValueOrDefault(false))
        {
            return;
        }

        var deleteGroupResult = await groupsApiClient.DeleteGroupAsync(new DeleteGroupCommand() { GroupId = groupToDelete.GroupID });
        if (deleteGroupResult.IsSuccess)
        {
            snackbar.Add("Gruppe erfolgreich gelöscht.", Severity.Success);
            await InitializeAsync();
            return;
        }
        else
        {
            snackbar.Add("Fehler beim Löschen der Gruppe."
                + Environment.NewLine + string.Join(", ", deleteGroupResult.ErrorMessages), Severity.Error);
        }
    }

    private async void GroupsViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(SelectedGroup):
                logger.LogInformation($"Selected group changed to {SelectedGroup?.Name}.");
                break;
        }
    }
}
