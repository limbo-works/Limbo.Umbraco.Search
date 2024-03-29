﻿using System;
using System.Diagnostics.CodeAnalysis;
using Examine;
using Limbo.Umbraco.Search.Models;
using Limbo.Umbraco.Search.Models.Groups;
using Limbo.Umbraco.Search.Options;
using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.Search;

/// <summary>
/// Interface describing a helper class for making Examine searches. See the <see cref="SearchHelper"/> class for a concrete implementation.
/// </summary>
public interface ISearchHelper {

    /// <summary>
    /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
    /// </summary>
    /// <param name="options">The options for the search.</param>
    /// <returns>An instance of <see cref="SearchResultList"/> representing the result of the search.</returns>
    SearchResultList Search(ISearchOptions options);

    /// <summary>
    /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
    ///
    /// Each item in the result is parsed to the type of <typeparamref name="TItem"/> using <paramref name="callback"/>.
    /// </summary>
    /// <typeparam name="TItem">The common output type of each item.</typeparam>
    /// <param name="options">The options for the search.</param>
    /// <param name="callback">A callback used for converting an <see cref="ISearchResult"/> to <typeparamref name="TItem"/>.</param>
    /// <returns>An instance of <see cref="SearchResultList{TItem}"/> representing the result of the search.</returns>
    SearchResultList<TItem> Search<TItem>(ISearchOptions options, Func<ISearchResult, TItem> callback);

    /// <summary>
    /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
    ///
    /// Each item in the result first found in either the content cache or media cache, and then parsed to the type of <typeparamref name="TItem"/> using <paramref name="callback"/>.
    /// </summary>
    /// <typeparam name="TItem">The common output type of each item.</typeparam>
    /// <param name="options">The options for the search.</param>
    /// <param name="callback">A callback used for converting an <see cref="IPublishedContent"/> to <typeparamref name="TItem"/>.</param>
    /// <returns>An instance of <see cref="SearchResultList{TItem}"/> representing the result of the search.</returns>
    SearchResultList<TItem> Search<TItem>(ISearchOptions options, Func<IPublishedContent, TItem> callback);

    /// <summary>
    /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
    ///
    /// Each item in the result first found in either the content cache or media cache, and then parsed to the type of <typeparamref name="TItem"/> using <paramref name="callback"/>.
    /// </summary>
    /// <typeparam name="TItem">The common output type of each item.</typeparam>
    /// <param name="options">The options for the search.</param>
    /// <param name="callback">A callback used for converting an <see cref="IPublishedContent"/> to <typeparamref name="TItem"/>.</param>
    /// <returns>An instance of <see cref="SearchResultList{TItem}"/> representing the result of the search.</returns>
    SearchResultList<TItem> Search<TItem>(ISearchOptions options, Func<IPublishedContent, ISearchResult, TItem> callback);

    /// <summary>
    /// Performs a search based on the specified <paramref name="request"/>.
    /// </summary>
    /// <param name="request">The request the search should be based on.</param>
    /// <param name="groups">An array of groups to used for the search.</param>
    /// <returns>An instance of <see cref="GroupedSearchResult"/>.</returns>
    GroupedSearchResult Search(HttpRequest request, SearchGroup[] groups);

    /// <summary>
    /// Replaces and removes diacritics in the specified <paramref name="input"/> string.
    /// </summary>
    /// <param name="input">The string.</param>
    /// <returns>The result of the replacement.</returns>
    string ReplaceDiacritics(string input);

    /// <summary>
    /// Removes diacritics in the specified <paramref name="input"/> string.
    /// </summary>
    /// <param name="input">The string.</param>
    /// <returns>The result of the operation.</returns>
    string RemoveDiacritics(string input);

    /// <summary>
    /// Returns a normalized version of the specified <paramref name="input"/> string.
    ///
    /// The purpose of this method is to either replace or strip special characters to improve the search
    /// experience. For instance if this method is used for both the indexed values and the search text, a value
    /// like <c>héllo</c> may converted to <c>hello</c>, improving the search regarding accents and diacritics.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The normalized version of the <paramref name="input"/> string.</returns>
    [return: NotNullIfNotNull("input")]
    string? NormalizeString(string? input);

}