using BCS.AdminSearch.Drivers;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace BCS.AdminSearch
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer<AdminMenu> T;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask;

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["Settings"], settings => settings
                        .Add(T["Admin Search"], T["Admin Search"], entry => entry
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = AdminSearchSettingsDisplayDriver.GroupId })
                            .Permission(Permissions.ManageAdminSearchSettings)
                            .LocalNav()
                        )))
                .Add(T["Content"], content => content
                    .Add(T["Search"], T["Search"], search => search
                        .Action("Search", "Admin", new { area = "BCS.AdminSearch" })
                        .LocalNav()
                    ));

            return Task.CompletedTask;
        }
    }
}
