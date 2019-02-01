using System.Collections.Generic;

namespace BCS.AdminSearch.ViewModels
{
    public class AdminSearchSettingsViewModel
    {
        public string AdminSearchIndex { get; set; }
        public IEnumerable<string> SearchIndexes { get; set; }
        public string AdminSearchFields { get; set; }
    }
}
