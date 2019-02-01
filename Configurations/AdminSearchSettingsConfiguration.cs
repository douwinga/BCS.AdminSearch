using BCS.AdminSearch.Models;
using Microsoft.Extensions.Options;
using OrchardCore.Entities;
using OrchardCore.Settings;

namespace BCS.AdminSearch.Configurations
{
    public class AdminSearchSettingsConfiguration : IConfigureOptions<AdminSearchSettings>
    {
        private readonly ISiteService _site;

        public AdminSearchSettingsConfiguration(ISiteService site)
        {
            _site = site;
        }

        public void Configure(AdminSearchSettings options)
        {
            var settings = _site.GetSiteSettingsAsync()
                .GetAwaiter().GetResult()
                .As<AdminSearchSettings>();

            options.AdminSearchIndex = settings.AdminSearchIndex;
            options.AdminSearchFields = settings.AdminSearchFields;
        }
    }
}
