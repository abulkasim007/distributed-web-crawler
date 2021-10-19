using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;

namespace SearchIndexerWorker
{
    public class SearchController : ControllerBase
    {
        private readonly SearchIndex searchIndex;

        public SearchController(SearchIndex searchIndex)
        {
            this.searchIndex = searchIndex;
        }

        [HttpGet]
        public IEnumerable<Result> Get(string query)
        {
            return searchIndex.Search(query);
        }
    }
}
