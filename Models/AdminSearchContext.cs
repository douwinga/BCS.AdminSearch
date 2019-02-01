using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace BCS.AdminSearch.Models
{
    public class AdminSearchContext
    {
        public string SearchTerm { get; set; }
        public IEnumerable<KeyValuePair<string, string[]>> Filters { get; set; }

        [BindNever]
        public string Index { get; set; }
        [BindNever]
        public string[] ContentTypes { get; set; }
        [BindNever]
        public string[] IndexFieldsToSearch { get; set; }
        [BindNever]
        public int PageSize { get; set; }
        [BindNever]
        public int PageNumber { get; set; }
    }
}
