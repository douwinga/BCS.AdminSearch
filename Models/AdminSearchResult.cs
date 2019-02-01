using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace BCS.AdminSearch.Models
{
    public class AdminSearchResult
    {
        public IEnumerable<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
        public int TotalRecordCount { get; set; } = 0;
    }
}
