using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Specialized;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultQueryBuilder : IQueryBuilder
    {
        public NameValueCollection BuildQueryString(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SearchQuery BuildSearchQuery(NameValueCollection queryString, Item searchSettings)
        {
            throw new NotImplementedException();
        }
    }
}
