using BCS.AdminSearch.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace BCS.AdminSearch.ViewModels
{
    public class AdminSearchViewModel
    {
        public string IndexFieldsToSearch { get; set; }
        public string Index { get; set; }
        public string[] ContentTypes { get; set; } = new string[0];

        [BindNever]
        public string SearchTerm { get; set; }
        [BindNever]
        public int TotalRecordCount { get; set; }
        [BindNever]
        public int PageSize { get; set; }
        [BindNever]
        public int PageNumber { get; set; }
        [BindNever]
        public IEnumerable<ContentItem> ContentItems { get; set; }
        [BindNever]
        public dynamic Pager { get; set; }
        [BindNever]
        public string[] Indices { get; set; }
        [BindNever]
        public IEnumerable<AdminSearchFilter> SearchFilters { get; set; }
        [BindNever]
        public IEnumerable<KeyValuePair<string, string[]>> Filters { get; set; }
    }
}
