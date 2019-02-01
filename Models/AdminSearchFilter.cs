using System.Collections.Generic;

namespace BCS.AdminSearch.Models
{
    public class AdminSearchFilter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IndexField { get; set; }
        public List<AdminSearchFilterOption> Options { get; set; }
    }
}
