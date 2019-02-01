using BCS.AdminSearch.Models;
using Lucene.Net.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCS.AdminSearch.Services
{
    public interface IAdminSearchFilter
    {
        Task Filter(AdminSearchContext searchContext, BooleanFilter booleanFilter);
        Task<IEnumerable<AdminSearchFilter>> GetFilter(string[] contentType);
    }
}
