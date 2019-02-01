using BCS.AdminSearch.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Queries;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using OrchardCore.ContentManagement;
using OrchardCore.Lucene;
using OrchardCore.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCS.AdminSearch.Services
{
    public class AdminSearchService : IAdminSearchService
    {
        private readonly LuceneIndexManager _luceneIndexProvider;
        private readonly IContentManager _contentManager;
        private readonly IEnumerable<IAdminSearchFilter> _searchFilters;

        public AdminSearchService(LuceneIndexManager luceneIndexProvider,
            IContentManager contentManager,
            IEnumerable<IAdminSearchFilter> searchFilters)
        {
            _luceneIndexProvider = luceneIndexProvider;
            _contentManager = contentManager;
            _searchFilters = searchFilters;
        }

        public async Task<AdminSearchResult> SearchContent(AdminSearchContext searchContext)
        {
            var searchResult = new AdminSearchResult();

            await _luceneIndexProvider.SearchAsync(searchContext.Index, async searcher =>
            {
                Query query;
                if (string.IsNullOrWhiteSpace(searchContext.SearchTerm))
                {
                    query = new MatchAllDocsQuery();
                }
                else
                {
                    var luceneVersion = LuceneSettings.DefaultVersion;
                    var analyzer = new StandardAnalyzer(luceneVersion);

                    var multiFieldQuery = new MultiFieldQueryParser(luceneVersion, searchContext.IndexFieldsToSearch, analyzer);
                    query = multiFieldQuery.Parse(QueryParserBase.Escape(searchContext.SearchTerm));
                }

                searchContext.PageNumber -= 1;

                var start = searchContext.PageNumber * searchContext.PageSize;
                var end = searchContext.PageNumber * searchContext.PageSize + searchContext.PageSize;

                var collector = TopScoreDocCollector.Create(end, true);
                var filter = new BooleanFilter();

                if (searchContext.ContentTypes.Any())
                {
                    filter.Add(new FieldCacheTermsFilter("Content.ContentItem.ContentType", searchContext.ContentTypes), Occur.MUST);
                }

                if (searchContext.Filters != null && searchContext.Filters.Any())
                {
                    await _searchFilters.InvokeAsync(x => x.Filter(searchContext, filter), null);
                }

                if (filter.Any())
                {
                    searcher.Search(query, filter, collector);
                }
                else
                {
                    searcher.Search(query, collector);
                }

                var docs = collector.GetTopDocs(start, end);
                searchResult.TotalRecordCount = docs.TotalHits;

                var contentItemIds = docs.ScoreDocs.Select(hit =>
                {
                    var doc = searcher.Doc(hit.Doc);
                    return doc.GetField("ContentItemId").GetStringValue();
                });

                searchResult.ContentItems = await _contentManager.GetAsync(contentItemIds);
            });

            return searchResult;
        }
    }
}
