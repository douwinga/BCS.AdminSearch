using BCS.AdminSearch.Models;
using BCS.AdminSearch.Services;
using BCS.AdminSearch.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCS.AdminSearch.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminSearchService _searchService;
        private readonly ISiteService _siteService;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IEnumerable<IAdminSearchFilter> _adminSearchFilters;
        private readonly AdminSearchSettings _adminSearchSettings;

        public AdminController(
            IAdminSearchService searchService,
            ISiteService siteService,
            IContentDefinitionManager contentDefinitionManager,
            IEnumerable<IAdminSearchFilter> adminSearchFilters,
            IShapeFactory shapeFactory,
            IOptions<AdminSearchSettings> adminSearchSettings
            )
        {
            _searchService = searchService;
            _siteService = siteService;
            _contentDefinitionManager = contentDefinitionManager;
            _adminSearchFilters = adminSearchFilters;
            _adminSearchSettings = adminSearchSettings.Value;

            New = shapeFactory;
        }

        public dynamic New { get; set; }

        public async Task<IActionResult> Search(PagerParameters pagerParameters, IList<KeyValuePair<string, string[]>> filters, string searchTerm)
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();

            var searchContext = new AdminSearchContext
            {
                Index = _adminSearchSettings.AdminSearchIndex,
                ContentTypes = _contentDefinitionManager.ListTypeDefinitions().Select(c => c.Name).ToArray(), // TODO: Once content types have a setting for which indexes they use, this should be updated to obey that
                IndexFieldsToSearch = _adminSearchSettings.AdminSearchFields,
                PageSize = pagerParameters.PageSize ?? siteSettings.PageSize,
                PageNumber = pagerParameters.Page ?? 1,
                Filters = filters,
                SearchTerm = searchTerm
            };

            var searchResults = new AdminSearchResult();

            if (filters.Any() || !string.IsNullOrWhiteSpace(searchTerm))
            {
                searchResults = await _searchService.SearchContent(searchContext);
            }

            var model = new AdminSearchViewModel
            {
                SearchFilters = await _adminSearchFilters.InvokeAsync(x => x.GetFilter(searchContext.ContentTypes), null),
                SearchTerm = searchContext.SearchTerm,
                ContentItems = searchResults.ContentItems,
                TotalRecordCount = searchResults.TotalRecordCount,
                PageSize = searchContext.PageSize,
                PageNumber = searchContext.PageNumber,
                Filters = searchContext.Filters
            };

            var pager = new Pager(pagerParameters, siteSettings.PageSize);
            var pagerShape = (await New.Pager(pager))
                .TotalItemCount(searchResults.TotalRecordCount)
                .ShowNext(searchResults.TotalRecordCount > searchContext.PageSize * (searchContext.PageNumber + 1));

            model.Pager = pagerShape;

            return View(model);
        }
    }
}
