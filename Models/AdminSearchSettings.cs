namespace BCS.AdminSearch.Models
{
    public class AdminSearchSettings
    {
        public string AdminSearchIndex { get; set; }
        public string[] AdminSearchFields { get; set; } = new string[0];
    }
}
