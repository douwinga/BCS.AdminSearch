using BCS.AdminSearch.Models;
using BCS.AdminSearch.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Lucene;
using OrchardCore.Settings;
using System;
using System.Threading.Tasks;

namespace BCS.AdminSearch.Drivers
{
    public class AdminSearchSettingsDisplayDriver : SectionDisplayDriver<ISite, AdminSearchSettings>
    {
        public const string GroupId = "AdminSearch";

        private readonly LuceneIndexManager _luceneIndexProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public AdminSearchSettingsDisplayDriver(
            LuceneIndexManager luceneIndexProvider,
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService
            )
        {
            _luceneIndexProvider = luceneIndexProvider;
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        public override async Task<IDisplayResult> EditAsync(AdminSearchSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageAdminSearchSettings))
            {
                return null;
            }

            return Initialize<AdminSearchSettingsViewModel>("AdminSearchSettings_Edit", model =>
            {
                model.AdminSearchIndex = section.AdminSearchIndex;
                model.AdminSearchFields = String.Join(", ", section.AdminSearchFields ?? new string[0]);
                model.SearchIndexes = _luceneIndexProvider.List();
            }).Location("Content:2").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(AdminSearchSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageAdminSearchSettings))
            {
                return null;
            }

            if (context.GroupId == GroupId)
            {
                var model = new AdminSearchSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                section.AdminSearchIndex = model.AdminSearchIndex;
                section.AdminSearchFields = model.AdminSearchFields?.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return await EditAsync(section, context);
        }
    }
}
