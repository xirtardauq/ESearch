using System;
using System.Linq;
using System.Collections.Generic;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using SearchSettingsTemplate = ESearch.Foundation.Indexing;
using ESearch.Foundation.Indexing.Models;

namespace ESearch.Feature.SearchResultSummary.Models
{
    public class SearchResultSummaryModel
    {
        #region Property
        public int TotalCount { get; set; }
        public string Keywords { get; set; }
        public SearchQuery SearchQuery { get; set; }
        public bool IsDisplaySearchConditions { get; set; }
        #endregion

        #region Constructor
        public SearchResultSummaryModel(SearchQuery searchQuery, Item searchSettings, int totalCount)
        {
            TotalCount = totalCount;
            Keywords = string.Join(", ", SearchQuery.KeywordCondition.Keywords);
            SearchQuery = searchQuery;
            IsDisplaySearchConditions = !string.IsNullOrEmpty(Keywords)
                                  || SearchQuery.EqualsConditions.Count != 0
                                  || SearchQuery.ContainsConditions.Count != 0
                                  || SearchQuery.BetweenConditions.Count != 0;

            if (!IsDisplaySearchConditions) return;
            var dateFormat = searchSettings[SearchSettingsTemplate.Templates.SearchSettings.Fields.DateFormat];

            foreach (var equalCondition in SearchQuery.EqualsConditions ?? Enumerable.Empty<EqualsCondition>())
            {
                equalCondition.Value = GetValue(equalCondition.Value, dateFormat);
            }

            foreach (var betweenCondition in SearchQuery.BetweenConditions ?? Enumerable.Empty<BetweenCondition>())
            {

                betweenCondition.LowerValue = GetValue(betweenCondition.LowerValue, dateFormat);
                betweenCondition.UpperValue = GetValue(betweenCondition.UpperValue, dateFormat);
            }

            foreach (var containCondition in SearchQuery.ContainsConditions ?? Enumerable.Empty<ContainsCondition>())
            {
                //ICollection<string> collection = containCondition.Values.Select(value => GetValue(value, dateFormat)).ToList();
                containCondition.Values = containCondition.Values.Select(value => GetValue(value, dateFormat)).ToList();
            }

        }
        #endregion

        #region Method
        protected virtual string GetValue(string value, string dateFormat)
        {
            if (ID.TryParse(value, out var id))
            {
                var item = Context.Database.GetItem(id);
                return item?.DisplayName ?? value;
            }

            if (ShortID.TryParse(value, out var shortId))
            {
                var item = Context.Database.GetItem(shortId.ToID());
                return item?.DisplayName ?? value;
            }

            if (DateTime.TryParse(value, out var date))
            {
                return date.ToLocalTime().ToString(dateFormat);
            }
            return value;
        }
        #endregion
    }
}
