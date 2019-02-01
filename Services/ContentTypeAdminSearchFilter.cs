using BCS.AdminSearch.Models;
using Lucene.Net.Queries;
using Lucene.Net.Search;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCS.AdminSearch.Services
{
    public class ContentTypeAdminSearchFilter : IAdminSearchFilter
    {
        private string IndexField => "Content.ContentItem.ContentType";

        public Task Filter(AdminSearchContext searchContext, BooleanFilter booleanFilter)
        {
            var contentTypeFilter = searchContext.Filters.FirstOrDefault(x => x.Key == IndexField);

            if (contentTypeFilter.Value == null)
            {
                return Task.CompletedTask;
            }

            booleanFilter.Add(new FilterClause(new FieldCacheTermsFilter(contentTypeFilter.Key, contentTypeFilter.Value), Occur.MUST));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<AdminSearchFilter>> GetFilter(string[] contentTypes)
        {
            var searchFilters = new List<AdminSearchFilter>
            {
                new AdminSearchFilter
                {
                    Name = "Content Type",
                    Type = "ContentType",
                    IndexField = IndexField,
                    Options = contentTypes.Select(c => new AdminSearchFilterOption
                        {
                            Name = c,
                            Value = c
                        }).ToList()
                }
            };

            return Task.FromResult<IEnumerable<AdminSearchFilter>>(searchFilters);
        }
    }
}
