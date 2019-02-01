using BCS.AdminSearch.Configurations;
using BCS.AdminSearch.Drivers;
using BCS.AdminSearch.Models;
using BCS.AdminSearch.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;

namespace BCS.AdminSearch
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IAdminSearchService, AdminSearchService>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<IDisplayDriver<ISite>, AdminSearchSettingsDisplayDriver>();

            services.AddTransient<IConfigureOptions<AdminSearchSettings>, AdminSearchSettingsConfiguration>();

            // Search Filter Providers
            services.AddScoped<IAdminSearchFilter, ContentTypeAdminSearchFilter>();
        }
    }
}
