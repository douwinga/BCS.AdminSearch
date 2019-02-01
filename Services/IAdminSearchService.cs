using BCS.AdminSearch.Models;
using System.Threading.Tasks;

namespace BCS.AdminSearch.Services
{
    public interface IAdminSearchService
    {
        Task<AdminSearchResult> SearchContent(AdminSearchContext searchContext);
    }
}
