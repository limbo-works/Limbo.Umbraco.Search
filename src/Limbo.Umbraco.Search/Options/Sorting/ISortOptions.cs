using Examine;
using Examine.Search;
using Skybrud.Essentials.Collections;

namespace Limbo.Umbraco.Search.Options.Sorting {

    /// <summary>
    /// Interface used for decribing how a collection of <see cref="ISearchResult"/> should be sorted during the search.
    /// </summary>
    public interface ISortOptions : ISearchOptions {

        /// <summary>
        /// The property field to sort after.
        /// </summary>
        string SortField { get; set; }

        /// <summary>
        /// The sort type of the property field.
        /// </summary>
        /// <remarks>
        /// For FullTextSortable use <see cref="Examine.Search.SortType.String"/> and for DateTime use
        /// <see cref="Examine.Search.SortType.Long"/>.
        /// </remarks>
        SortType SortType { get; set; }

        /// <summary>
        /// Gets or sets the order by which the results should be sorted. Possible values are
        /// <see cref="Skybrud.Essentials.Collections.SortOrder.Ascending"/> and
        /// <see cref="Skybrud.Essentials.Collections.SortOrder.Descending"/>.
        /// </summary>
        SortOrder SortOrder { get; set; }

    }

}