using System.Collections.Generic;
using Examine;
using Examine.Search;
using Limbo.Umbraco.Search.Options;
using Limbo.Umbraco.Search.Options.Pagination;

namespace Limbo.Umbraco.Search.Models {

    /// <summary>
    /// Class representing a result of a search based on <see cref="Options"/>.
    /// </summary>
    public class SearchResultList {

        #region Properties

        /// <summary>
        /// Gets the options the search was based on.
        /// </summary>
        public ISearchOptions Options { get; }

        /// <summary>
        /// Gets whether debugging was enabled for the search.
        /// </summary>
        public bool IsDebug { get; }

        /// <summary>
        /// 
        /// </summary>
        public IQuery Query { get; }

        /// <summary>
        /// Gets the total amount of items returned by the search.
        /// </summary>
        public long Total { get; }

        /// <summary>
        /// Gets the items returned by the search.
        ///
        /// If <see cref="Options"/> implements <see cref="IOffsetOptions"/>, the items will be paginated honouring
        /// <see cref="IOffsetOptions.Limit"/> and <see cref="IOffsetOptions.Offset"/>.
        /// </summary>
        public IEnumerable<ISearchResult> Items { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new result list based on the specified <paramref name="options"/>, <paramref name="query"/>,
        /// <paramref name="total"/> count and <paramref name="items"/>.
        /// </summary>
        /// <param name="options">The options used for making the search.</param>
        /// <param name="query">The search query.</param>
        /// <param name="total">The total amount of results returned by the search.</param>
        /// <param name="items">The items representing the result of the search.</param>
        public SearchResultList(ISearchOptions options, IQuery query, long total, IEnumerable<ISearchResult> items) {
            Options = options;
            IsDebug = options is IDebugSearchOptions { IsDebug: true };
            Query = query;
            Total = total;
            Items = items;
        }

        #endregion

    }

}