using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using Limbo.Umbraco.Search.Constants;
using Limbo.Umbraco.Search.Extensions;

namespace Limbo.Umbraco.Search.Options {

    /// <summary>
    /// Static class with various extension methods for <see cref="QueryList"/>.
    /// </summary>
    public static class QueryListExtensions {

        /// <summary>
        /// Appends the query with the specified <paramref name="nodeTypeAlias"/> to the query list.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="nodeTypeAlias">The alias of the node type.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendNodeTypeAlias<T>(this T? list, string nodeTypeAlias) where T : QueryList {
            if (string.IsNullOrWhiteSpace(nodeTypeAlias)) throw new ArgumentNullException(nameof(nodeTypeAlias));
            list?.Add($"{ExamineFields.NodeTypeAlias}:{nodeTypeAlias}");
            return list;
        }

        /// <summary>
        /// Appends a new OR query for matching one of the specified <paramref name="nodeTypeAliases"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="nodeTypeAliases">The aliases of the node types.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendNodeTypeAliases<T>(this T? list, params string[]? nodeTypeAliases) where T : QueryList {
            if (nodeTypeAliases == null || nodeTypeAliases.Length == 0) return list;
            list?.Add($"{ExamineFields.NodeTypeAlias}:({string.Join(" ", nodeTypeAliases)})");
            return list;
        }

        /// <summary>
        /// Appends a new OR query for matching one of the specified <paramref name="nodeTypeAliases"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="nodeTypeAliases">The aliases of the node types.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendNodeTypeAliases<T>(this T? list, IEnumerable<string>? nodeTypeAliases) where T : QueryList {
            return nodeTypeAliases == null ? list : AppendNodeTypeAliases(list, nodeTypeAliases.ToArray());
        }

        /// <summary>
        /// Appends a new OR query for matching one of the specified <paramref name="contentTypes"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="contentTypes">A list of content types.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendNodeTypeAliases<T>(this T? list, ContentTypeList? contentTypes) where T : QueryList {
            if (contentTypes == null || contentTypes.Count == 0) return list;
            list?.Add($"{ExamineFields.NodeTypeAlias}:({string.Join(" ", contentTypes.ToArray())})");
            return list;
        }

        /// <summary>
        /// Appends a new query requiring that results doesn't have a flag that they should be hidden from search results.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendHideFromSearch<T>(this T? list) where T : QueryList {
            list?.Add($"{ExamineFields.HideFromSearch}:0");
            return list;
        }

        /// <summary>
        /// Appends a new query requiring that the returned result are a descendant of or equal to the node with the specified <paramref name="ancestorId"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="ancestorId">The ID of the ancestor.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendAncestor<T>(this T? list, int ancestorId) where T : QueryList {
            list?.Add($"{ExamineFields.PathSearch}:{ancestorId}");
            return list;
        }

        /// <summary>
        /// Appends a new OR query requiring that returned results are a descendant or equal to at least one of the nodes matching the specified <paramref name="ancestorIds"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="ancestorIds">The IDs of the ancestors.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendAncestors<T>(this T? list, params int[]? ancestorIds) where T : QueryList {
            if (ancestorIds == null || ancestorIds.Length == 0) return list;
            list?.Add($"{ExamineFields.PathSearch}:({string.Join(" ", from id in ancestorIds select id)})");
            return list;
        }

        /// <summary>
        /// Appends a new OR query requiring that returned results are a descendant or equal to at least one of the nodes matching the specified <paramref name="ancestorIds"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="ancestorIds">The IDs of the ancestors.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? AppendAncestors<T>(this T? list, IEnumerable<int>? ancestorIds) where T : QueryList {
            return ancestorIds == null ? null : AppendAncestors(list, ancestorIds.ToArray());
        }

        /// <summary>
        /// Appends the specified <paramref name="queryList"/> to the query list
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list to which <paramref name="queryList"/> should be added.</param>
        /// <param name="queryList">The query list to be added.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        [return: NotNullIfNotNull("list")]
        public static T? Append<T>(this T? list, QueryList queryList) where T : QueryList {
            list?.Add(queryList);
            return list;
        }

        /// <summary>
        /// Appends a new sub query to <paramref name="list"/> for the specified <paramref name="ids"/>. If multiple
        /// IDs are specified, the search is OR-based.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list. If set to <c>path</c>, the searched field will be <c>path_search</c>.</param>
        /// <param name="field">The key of the field. If </param>
        /// <param name="ids">The IDs to search for.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexCsv"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendIds<T>(this T? list, string field, IEnumerable<int>? ids) where T : QueryList {

            if (list is null || ids is null) return list;

            int i = 0;

            StringBuilder sb = new();

            sb.Append($"{field}_search");
            sb.Append(":(");
            foreach (int id in ids) {
                if (i > 0) sb.Append(" OR ");
                sb.Append(id);
                i++;
            }
            sb.Append(')');

            if (i > 0) list.Add(sb.ToString());

            return list;

        }

        /// <summary>
        /// Appends a new sub query to <paramref name="list"/> for the specified <paramref name="guids"/>. If multiple
        /// GUID values are specified, the search is OR-based.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="field">The key of the field. If set to <c>keys</c>, the searched field will be <c>keys_search</c>.</param>
        /// <param name="guids">The GUID values to search for.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexCsv"/> or <see cref="ExamineIndexingExtensions.IndexUdis"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendGuids<T>(this T? list, string field, IEnumerable<Guid>? guids) where T : QueryList {

            if (list is null || guids is null) return list;

            int i = 0;

            StringBuilder sb = new();

            sb.Append($"{field}_search");
            sb.Append(":(");
            foreach (Guid guid in guids) {
                if (i > 0) sb.Append(" OR ");
                sb.Append($"{guid:N}");
                i++;
            }
            sb.Append(')');

            if (i > 0) list.Add(sb.ToString());

            return list;

        }

        /// <summary>
        /// Appends a new date range sub query based on the specified <paramref name="min"/> and <paramref name="max"/> dates.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="field">The key of the field. If set to <c>contentDate</c>, the searched field will be <c>contentDate_search</c>.</param>
        /// <param name="min">The minimum date to search by. Uses <see cref="DateTime.MinValue"/> as fallback if specified value is <see langword="null"/>.</param>
        /// <param name="max">The maximum date to search by. Uses <see cref="DateTime.MaxValue"/> as fallback if specified value is <see langword="null"/>.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexDateExtended"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendDateRange<T>(this T? list, string field, DateTime? min, DateTime? max) where T : QueryList {
            string minValue = (min ?? DateTime.MinValue).ToString(ExamineDateFormats.Sortable, CultureInfo.InvariantCulture);
            string maxValue = (max ?? DateTime.MaxValue).ToString(ExamineDateFormats.Sortable, CultureInfo.InvariantCulture);
            list?.Add($"{field}_search:[{minValue} TO {maxValue}]");
            return list;
        }

        /// <summary>
        /// Appends a new date year sub query based on the specified <paramref name="year"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="field">The key of the field. If set to <c>contentDate</c>, the searched field will be <c>contentDate_year</c>.</param>
        /// <param name="year">The year to search for.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexDateExtended"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendDateYear<T>(this T? list, string field, int year) where T : QueryList {
            list?.Add($"{field}_search:[{year} TO {year}]");
            return list;
        }

        /// <summary>
        /// Appends a new date year sub query based on the specified <paramref name="date"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="field">The key of the field. If set to <c>contentDate</c>, the searched field will be <c>contentDate_year</c>.</param>
        /// <param name="date">A date representing the year to search for.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexDateExtended"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendDateYear<T>(this T? list, string field, DateTime date) where T : QueryList {
            return AppendDateYear(list, field, date.Year);
        }

        /// <summary>
        /// Appends a new date year sub query based on the specified <paramref name="min"/> and <paramref name="max"/> years.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="field">The key of the field. If set to <c>contentDate</c>, the searched field will be <c>contentDate_year</c>.</param>
        /// <param name="min">The minimum date to search by. Uses <see cref="DateTime.MinValue"/> as fallback if specified value is <see langword="null"/>.</param>
        /// <param name="max">The maximum date to search by. Uses <see cref="DateTime.MaxValue"/> as fallback if specified value is <see langword="null"/>.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexDateExtended"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendDateYearRange<T>(this T? list, string field, int? min, int? max) where T : QueryList {
            list?.Add($"{field}_search:[{min ?? 0} TO {max ?? 9999}]");
            return list;
        }

        /// <summary>
        /// Appends a new date year sub query based on the specified <paramref name="min"/> and <paramref name="max"/> years.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="field">The key of the field. If set to <c>contentDate</c>, the searched field will be <c>contentDate_year</c>.</param>
        /// <param name="min">The minimum date to search by. Uses <see cref="DateTime.MinValue"/> as fallback if specified value is <see langword="null"/>.</param>
        /// <param name="max">The maximum date to search by. Uses <see cref="DateTime.MaxValue"/> as fallback if specified value is <see langword="null"/>.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        /// <remarks>Use together with the <see cref="ExamineIndexingExtensions.IndexDateExtended"/> methods.</remarks>
        [return: NotNullIfNotNull("list")]
        public static T? AppendDateYearRange<T>(this T? list, string field, DateTime? min, DateTime? max) where T : QueryList {
            return AppendDateYearRange(list, field, min?.Year, max?.Year);
        }

    }

}