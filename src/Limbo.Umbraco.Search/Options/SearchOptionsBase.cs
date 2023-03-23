using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Examine;
using Examine.Search;
using Limbo.Umbraco.Search.Constants;
using Limbo.Umbraco.Search.Options.Fields;

namespace Limbo.Umbraco.Search.Options {

    /// <summary>
    /// Base class implementing the <see cref="ISearchOptions"/>, <see cref="IGetSearcherOptions"/> and <see cref="IDebugSearchOptions"/> interfaces.
    /// </summary>
    public class SearchOptionsBase : IGetSearcherOptions, IDebugSearchOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the text to search for.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets an array of IDs the returned results should be a descendant of. At least one of the IDs should
        /// be in the path of the result to be a match.
        /// </summary>
        public List<int> RootIds { get; set; }

        /// <summary>
        /// Gets or sets whether the check on the <c>hideFromSearch</c> field should be disabled.
        /// </summary>
        public bool DisableHideFromSearch { get; set; }

        /// <summary>
        /// Gets whether the search should be performed in debug mode.
        /// </summary>
        public bool IsDebug { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public SearchOptionsBase() {
            Text = string.Empty;
            RootIds = new List<int>();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns the <see cref="ISearcher"/> to be used for the search.
        /// </summary>
        /// <param name="examineManager">A reference to the current <see cref="IExamineManager"/>.</param>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <returns>An instance of <see cref="ISearcher"/>, or <c>null</c> if the options class doesn't explicitly specify a searcher.</returns>
        public virtual ISearcher GetSearcher(IExamineManager examineManager, ISearchHelper searchHelper) {
            return GetSearcherByIndexName(examineManager, searchHelper, ExamineIndexes.ExternalIndexName);
        }

        /// <summary>
        /// Returns the <see cref="IBooleanOperation"/> to be used for the search.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="searcher">The <see cref="ISearcher"/> to be used for the search.</param>
        /// <param name="query">The current <see cref="IQuery"/>.</param>
        /// <returns>An instance of <see cref="IBooleanOperation"/>.</returns>
        public virtual IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query) {
            return query.NativeQuery(string.Join(" AND ", GetQueryList(searchHelper)));
        }

        /// <summary>
        /// Returns the <see cref="QueryList"/> to be used for search.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <returns>An insdtance of <see cref="QueryList"/>.</returns>
        protected virtual QueryList GetQueryList(ISearchHelper searchHelper) {

            QueryList query = new();

            SearchType(searchHelper, query);
            SearchText(searchHelper, query);
            SearchPath(searchHelper, query);
            SearchHideFromSearch(searchHelper, query);

            return query;

        }

        /// <summary>
        /// Returns a instance of <see cref="FieldList"/> indicating the fields that should be used for the text based search.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>An instance of <see cref="FieldList"/>.</returns>
        protected virtual FieldList GetTextFields(ISearchHelper helper) {
            return new FieldList {
                new("nodeName", 50),
                new("title", 40),
                new("teaser", 20)
            };
        }

        /// <summary>
        /// Virtual method for limiting the search to specific content types.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="query">The query.</param>
        protected virtual void SearchType(ISearchHelper searchHelper, QueryList query) { }

        /// <summary>
        /// Virtual method for configuring the text based search.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="query">The query.</param>
        protected virtual void SearchText(ISearchHelper searchHelper, QueryList query) {

            IReadOnlyList<string> terms = GetTerms(searchHelper, Text);
            if (terms.Count == 0) return;

            // Fallback if no fields are added
            FieldList fields = GetTextFields(searchHelper);
            if (fields.Count == 0) fields = FieldList.GetFromStringArray(new[] { "nodeName_lci", "contentTeasertext_lci", "contentBody_lci" });

            query.Add(fields.GetQuery(terms));

        }

        /// <summary>
        /// Returns a list of individual search terms (words) based on the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="text">The text to parse.</param>
        /// <returns>A list of individual terms (words).</returns>
        protected virtual IReadOnlyList<string> GetTerms(ISearchHelper searchHelper, string? text) {

            if (string.IsNullOrWhiteSpace(text)) return Array.Empty<string>();

            return Regex.Replace(text, @"[^\wæøåÆØÅ\-@\. ]", string.Empty)
                .ToLowerInvariant()
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        }

        /// <summary>
        /// Virtual method for limiting the search to specifi ancestors.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="query">The query.</param>
        protected virtual void SearchPath(ISearchHelper searchHelper, QueryList query) {
            if (RootIds.Count == 0) return;
            query.Add("(" + string.Join(" OR ", from id in RootIds select "path_search:" + id) + ")");
        }

        /// <summary>
        /// Virtual method for limiting the search to only searchable nodes (without the <c>hideFromSearch</c> flag).
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="query">The query.</param>
        protected virtual void SearchHideFromSearch(ISearchHelper searchHelper, QueryList query) {
            if (DisableHideFromSearch) return;
            query.Add("hideFromSearch:0");
        }

        /// <summary>
        /// Virtual method for getting a searcher by it's parent <paramref name="indexName"/>.
        /// </summary>
        /// <param name="examineManager">A reference to the current <see cref="IExamineManager"/>.</param>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="indexName">The name of the parent index.</param>
        /// <returns>An instance of <see cref="ISearcher"/>.</returns>
        /// <exception cref="Exception">If a mathing index isn't found, or the index doesn't specify a searcher, this method will throw an exception.</exception>
        protected virtual ISearcher GetSearcherByIndexName(IExamineManager examineManager, ISearchHelper searchHelper, string indexName) {

            // Get the index from the Examine manager
            if (!examineManager.TryGetIndex(indexName, out IIndex index)) {
                throw new Exception($"Examine index {indexName} not found.");
            }

            // Get the searcher from the index
            ISearcher searcher = index.Searcher;
            if (searcher == null) throw new Exception("Examine index {indexName} does not specify a searcher.");

            // Return the searcher
            return searcher;

        }

        #endregion

    }

}