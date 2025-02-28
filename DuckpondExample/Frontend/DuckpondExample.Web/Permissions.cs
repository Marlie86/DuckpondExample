namespace DuckpondExample.Web
{
    /// <summary>
    /// Provides permission constants for the application.
    /// </summary>
    public static class Permissions
    {
        /// <summary>
        /// Permission to show users information for client administrators.
        /// </summary>
        public static string UsersShow { get; private set; } = "User.Client.Administrator.Show";

        /// <summary>
        /// Permission to show group information for client administrators.
        /// </summary>
        public static string GroupsShow { get; private set; } = "Groups.Client.Administrator.Show";

        /// <summary>
        /// Permission to delete group information for client administrators.
        /// </summary>
        public static string GroupsDelete { get; private set; } = "Groups.Client.Administrator.Delete";

        /// <summary>
        /// Permission to show the menu for client administrators.
        /// </summary>
        public static string MenuAdministratorShow { get; private set; } = "Menu.Client.Administrator.Show";

    }
}
