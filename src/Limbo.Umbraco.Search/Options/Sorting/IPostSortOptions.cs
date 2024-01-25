using System.Collections.Generic;
using Examine;
using Microsoft.Extensions.Logging;

namespace Limbo.Umbraco.Search.Options.Sorting;

/// <summary>
/// Interface used for decribing how a collection of <see cref="ISearchResult"/> should be sorted after the search has been executed.
/// </summary>
public interface IPostSortOptions : ISearchOptions {

    /// <summary>
    /// Performs sort on <paramref name="results"/> after they have been retrieved from Examine.
    /// </summary>
    /// <param name="results">The results to be sorted.</param>
    /// <param name="logger">A reference to the current logger.</param>
    /// <returns>The sorted results.</returns>
    IEnumerable<ISearchResult> Sort(IEnumerable<ISearchResult> results, ILogger logger);

}