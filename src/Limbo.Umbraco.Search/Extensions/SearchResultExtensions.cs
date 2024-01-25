using System;
using System.Diagnostics.CodeAnalysis;
using Examine;

using static Skybrud.Essentials.Strings.StringUtils;

namespace Limbo.Umbraco.Search.Extensions {

    /// <summary>
    /// Static class with extension methods for <see cref="ISearchResult"/>.
    /// </summary>
    public static class SearchResultExtensions {

        /// <summary>
        /// Returns the string value of the field with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <returns>An instance if <see cref="string"/> holding the field value if successful; otherwise, <see langword="null"/>.</returns>
        public static string? GetString(this ISearchResult searchResult, string key) {
            return searchResult.Values.TryGetValue(key, out string? result) ? result : null;
        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/> and convert it to a <see cref="int"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found, and if the value can be converted to a <see cref="int"/>; otherwise, <c>0</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key and the value can be converted to a <see cref="int"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetInt32(this ISearchResult? searchResult, string key, out int result) {
            result = default;
            return searchResult != null && searchResult.Values.TryGetValue(key, out string? str) && TryParseInt32(str, out result);
        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/> and convert it to a <see cref="int"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found, and if the value can be converted to a <see cref="int"/>; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key and the value can be converted to a <see cref="int"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetInt32(this ISearchResult? searchResult, string key, out int? result) {
            result = null;
            return searchResult != null && searchResult.Values.TryGetValue(key, out string? str) && TryParseInt32(str, out result);
        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/> and convert it to a <see cref="long"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found, and if the value can be converted to a <see cref="int"/>; otherwise, <c>0</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key and the value can be converted to a <see cref="long"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetInt64(this ISearchResult? searchResult, string key, out long result) {
            result = default;
            return searchResult != null && searchResult.Values.TryGetValue(key, out string? str) && TryParseInt64(str, out result);
        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/> and convert it to a <see cref="long"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found, and if the value can be converted to a <see cref="int"/>; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key and the value can be converted to a <see cref="long"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetInt64(this ISearchResult? searchResult, string key, out long? result) {
            result = null;
            return searchResult != null && searchResult.Values.TryGetValue(key, out string? str) && TryParseInt64(str, out result);
        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/> and convert it to a <see cref="Guid"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <see cref="Guid.Empty"/>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key and the value can be converted to a <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetGuid(this ISearchResult? searchResult, string key, out Guid result) {

            if (searchResult != null && searchResult.Values.TryGetValue(key, out string? str)) {
                return Guid.TryParse(str, out result);
            }

            result = default;
            return false;

        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/> and convert it to a <see cref="Guid"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the value set contains a field with the specified key and the value can be converted to a <see cref="Guid"/>; otherwise, <see langword="null"/>.</returns>
        public static bool TryGetGuid(this ISearchResult? searchResult, string key, out Guid? result) {

            if (searchResult != null && searchResult.Values.TryGetValue(key, out string? str)) {
                bool success = Guid.TryParse(str, out Guid value);
                result = success ? value : null;
                return success;
            }

            result = default;
            return false;

        }

        /// <summary>
        /// Attempts to get the value of the field with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the result contains a field with the specified key; otherwise, <see langword="null"/>.</returns>
        public static bool TryGetString(this ISearchResult? searchResult, string key,  [NotNullWhen(true)] out string? result) {

            if (searchResult != null && searchResult.Values.TryGetValue(key, out result)) {
                return true;
            }

            result = default;
            return false;

        }

    }

}