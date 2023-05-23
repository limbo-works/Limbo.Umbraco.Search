using System.Collections.Generic;
using System.Reflection;
using Limbo.Umbraco.Search.Options.Fields;
using Limbo.Umbraco.Search.Options;

namespace Limbo.Umbraco.Search.Debugging {

    /// <summary>
    /// Static class with various <see cref="ISearchHelper"/> extension methods that may be used for debugging.
    /// </summary>
    public static class SearchHelperDebugExtensions {

        /// <summary>
        /// Returns the <see cref="QueryList"/> of the specified <paramref name="options"/>, or <see langword="null"/> null if not available.
        /// </summary>
        /// <param name="searchHelper">An instance of <see cref="ISearchHelper"/>.</param>
        /// <param name="options">The Search options.</param>
        /// <returns>An instance of <see cref="QueryList"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static QueryList? GetQueryList(this ISearchHelper searchHelper, ISearchOptions options) {
            return options
                .GetType()
                .GetMethod("GetQueryList", BindingFlags.Instance | BindingFlags.NonPublic)?
                .Invoke(options, new object?[] { searchHelper }) as QueryList;
        }

        /// <summary>
        /// Returns the <see cref="FieldList"/> of the specified <paramref name="options"/>, or <see langword="null"/> null if not available.
        /// </summary>
        /// <param name="searchHelper">An instance of <see cref="ISearchHelper"/>.</param>
        /// <param name="options">The Search options.</param>
        /// <returns>An instance of <see cref="FieldList"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static FieldList? GetTextFields(this ISearchHelper searchHelper, ISearchOptions options) {
            return options
                .GetType()
                .GetMethod("GetTextFields", BindingFlags.Instance | BindingFlags.NonPublic)?
                .Invoke(options, new object?[] { searchHelper }) as FieldList;
        }

        /// <summary>
        /// Returns the search terms of the specified <paramref name="options"/>, or <see langword="null"/> null if not available.
        /// </summary>
        /// <param name="searchHelper">An instance of <see cref="ISearchHelper"/>.</param>
        /// <param name="options">The Search options.</param>
        /// <returns>An instance of <see cref="IReadOnlyList{String}"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static IReadOnlyList<string>? GetTerms(this ISearchHelper searchHelper, ISearchOptions options) {

            if (options is SearchOptionsBase b) {
                return b.GetTerms(searchHelper, b.Text);
            }

            string? text = options
                .GetType()
                .GetProperty("Text", BindingFlags.Instance | BindingFlags.Public)?
                .GetValue(options) as string;

            return GetTerms(searchHelper, options, text);

        }

        /// <summary>
        /// Returns the search terms resolved from the specified <paramref name="text"/>, or <see langword="null"/> null if not available.
        /// </summary>
        /// <param name="searchHelper">An instance of <see cref="ISearchHelper"/>.</param>
        /// <param name="options">The Search options.</param>
        /// <param name="text">The raw search text.</param>
        /// <returns>An instance of <see cref="IReadOnlyList{String}"/> if successful; otherwise, <see langword="null"/>.</returns>
        public static IReadOnlyList<string>? GetTerms(this ISearchHelper searchHelper, ISearchOptions options, string? text) {

            if (options is SearchOptionsBase b) {
                return b.GetTerms(searchHelper, text);
            }

            return options
                .GetType()
                .GetMethod("GetTerms", BindingFlags.Instance | BindingFlags.NonPublic)?
                .Invoke(options, new object?[] { searchHelper, text }) as IReadOnlyList<string>;

        }

    }

}