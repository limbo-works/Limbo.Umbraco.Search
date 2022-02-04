using System.Collections.Generic;
using Examine;
using Examine.Search;

namespace Limbo.Umbraco.Search.Options.Sorting {

    /// <summary>
    /// Interface describing a <see cref="Execute"/> method.
    /// </summary>
    public interface IExecuteOptions : ISearchOptions {

        /// <summary>
        /// Executes the specified <paramref name="operation"/>.
        /// </summary>
        /// <param name="operation">The <see cref="IBooleanOperation"/> to be executed.</param>
        /// <param name="results">When this method returns, holds the results of the search. The results may be paginated.</param>
        /// <param name="total">When this method returns, holds the total amount of results returned by the search.</param>
        void Execute(IBooleanOperation operation, out IEnumerable<ISearchResult> results, out long total);

    }

}