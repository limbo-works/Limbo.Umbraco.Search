using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Examine;
using Limbo.Umbraco.Search.Constants;
using Limbo.Umbraco.Search.Indexing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings;
using Skybrud.Essentials.Time.Iso8601;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Examine;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Search.Extensions {

    /// <summary>
    /// Static class with various extension methods for aiding indexing in Umbraco/Examine.
    /// </summary>
    public static class ExamineIndexingExtensions {

        private static readonly string[] _defaultLciFields = {
            ExamineFields.NodeName, ExamineFields.Title, ExamineFields.Teaser
        };

        /// <summary>
        /// Attemps to get the first string value of a field with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key; otherwise, <c>false</c>.</returns>
        public static bool TryGetString(this IndexingItemEventArgs e, string key, out string value) {
            value = e.ValueSet.Values.TryGetValue(key, out List<object> values) ? values.FirstOrDefault()?.ToString() : null;
            return value != null;
        }
        
        /// <summary>
        /// Attempts to get the first value of the field with the specified <paramref name="key"/>. If the value is not already an <see cref="int"/>, the method will try to convert it.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <c>0</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key; otherwise, <c>false</c>.</returns>
        public static bool TryGetInt32(this IndexingItemEventArgs e, string key, out int result) {

            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) {
                result = default;
                return false;
            }

            result = 0;

            switch (values.FirstOrDefault()) {

                case int numeric:
                    result = numeric;
                    return true;

                case string str:
                    if (!int.TryParse(str, out int temp)) return false;
                    result = temp;
                    return true;

                default:
                    return false;

            }

        }

        /// <summary>
        /// Attempts to get the first value of the field with the specified <paramref name="key"/>. If the value is not already an <see cref="int"/>, the method will try to convert it.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field.</param>
        /// <param name="result">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key; otherwise, <c>false</c>.</returns>
        public static bool TryGetInt32(this IndexingItemEventArgs e, string key, out int? result) {

            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) {
                result = default;
                return false;
            }

            result = null;

            switch (values.FirstOrDefault()) {

                case int numeric:
                    result = numeric;
                    return true;

                case string str:
                    if (!int.TryParse(str, out int temp)) return false;
                    result = temp;
                    return true;

                default:
                    return false;

            }

        }
        
        /// <summary>
        /// Adds a new field with <paramref name="key"/> and <paramref name="value"/> if the field does not already exist.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The new value.</param>
        public static IndexingItemEventArgs AddDefaultValue(this IndexingItemEventArgs e, string key, string value) {

            // Does the field already exist?
            if (e.ValueSet.Values.ContainsKey(key)) return e;

            // Add the default value
            e.ValueSet.TryAdd(key, value);

            return e;

        }

        /// <summary>
        /// If a field with <paramref name="key"/> doesn't already exist, a new field where the key is a combination of
        /// <paramref name="key"/> and <paramref name="suffix"/> will be added with <paramref name="value"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="suffix">The suffix for the key of the new field.</param>
        public static IndexingItemEventArgs AddDefaultValue(this IndexingItemEventArgs e, string key, string value, string suffix) {

            // Does the field already exist?
            if (e.ValueSet.Values.ContainsKey(key)) return e;

            // Add the default value
            e.ValueSet.TryAdd($"{key}{suffix}", value);

            return e;

        }

        /// <summary>
        /// Adds a search-friendly version of the <c>path</c> field where the IDs are separated by spaces instead of commas.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        public static IndexingItemEventArgs IndexPath(this IndexingItemEventArgs e) {
            return IndexCsv(e, ExamineFields.Path);
        }

        /// <summary>
        /// If a field with <paramref name="key"/> exists, a new field in which commas in the value has been replaced
        /// by spaces, making each value searchable.
        ///
        /// The key of the new field will use <c>_search</c> as suffix - eg. if <paramref name="key"/> is <c>path</c>,
        /// the new field will have the key <c>path_search</c>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field to make searchable.</param>
        public static IndexingItemEventArgs IndexCsv(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;

            // Get the first value and replace all commas with an empty space
            string value = values.FirstOrDefault()?.ToString()?.Replace(',', ' ');

            // Added the searchable value to the index
            e.ValueSet.TryAdd($"{key}_search", value);

            return e;

        }

        /// <summary>
        /// Parses the UDIs in the field with the specified <paramref name="key"/>, and adds a new field with
        /// searchable versions of the UDIs.
        ///
        /// Specifically the method will look for any GUID based UDI's, and then format the GUIDs to formats <c>N</c>
        /// and <c>D</c> - that is <c>00000000000000000000000000000000</c> and
        /// <c>00000000-0000-0000-0000-000000000000</c>. The type of the reference entity is not added to the new field.
        ///
        /// The key of the new field will use <c>_search</c> as suffix - eg. if <paramref name="key"/> is
        /// <c>related</c>, the new field will have the key <c>related_search</c>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field to make searchable.</param>
        public static IndexingItemEventArgs IndexUdis(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;

            // Get the first value of the field
            string value = values.FirstOrDefault()?.ToString();

            // Parse the UDI's and adds as GUIDs instead (both N and D formats)
            List<string> newValues = new();
            foreach (string piece in StringUtils.ParseStringArray(value)) {
                if (UdiParser.TryParse(piece, out GuidUdi udi)) {
                    newValues.Add(udi.Guid.ToString("N"));
                    newValues.Add(udi.Guid.ToString("D"));
                } else {
                    newValues.Add(piece.Split('/').Last());
                }
            }

            // Added the searchable value to the index
            e.ValueSet.TryAdd($"{key}_search", string.Join(" ", newValues));

            return e;

        }

        /// <summary>
        /// Adds a new field with the specified <paramref name="key"/> and <paramref name="value"/>. Examine doesn't support boolean, so the value will be indexed as either <c>1</c> or <c>0</c>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the new field.</param>
        /// <param name="value">The boolean value to index.</param>
        public static void AddBoolean(this IndexingItemEventArgs e, string key, bool value) {
            e.ValueSet.TryAdd(key, value ? "1" : "0");
        }

        /// <summary>
        /// If a field exists with the specified <paramref name="key"/>, and the value matches an integer, a GUID representation of the value will be added to a field with <paramref name="newKey"/>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the existing field to look for.</param>
        /// <param name="newKey">The key of the new field.</param>
        public static void AddInt32AsGuid(this IndexingItemEventArgs e, string key, string newKey) {
            if (e.TryGetInt32(key, out int value)) e.ValueSet.TryAdd(newKey, value.ToGuid());
        }

        /// <summary>
        /// Adds a searchable version of the date value in the field with the specified <paramref name="key"/>.
        ///
        /// The searchable value will be added in a new field using the <c>_range</c> prefix for the key (as it enables
        /// a ranged query) and the value will be formatted using <c>yyyyMMddHHmm00000</c>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        public static IndexingItemEventArgs IndexDate(this IndexingItemEventArgs e, string key) {
            return IndexDateWithFormat(e, ExamineDateFormats.Sortable, key);
        }

        /// <summary>
        /// Adds searchable versions of the date values in the fields with the specified <paramref name="keys"/>.
        ///
        /// The searchable values will be added in new fields using the <c>_range</c> prefix for the keys (as it enables
        /// a ranged query) and the value will be formatted using <c>yyyyMMddHHmm00000</c>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="keys">The keys of the fields.</param>
        public static IndexingItemEventArgs IndexDate(this IndexingItemEventArgs e, params string[] keys) {
            if (keys == null) return null;
            foreach (string key in keys) IndexDate(e, key);
            return e;
        }

        /// <summary>
        /// Adds a searchable version of the date value in the field with the specified <paramref name="key"/>.
        /// 
        /// The searchable value will be added in a new field using the <c>_range</c> prefix for the key (at it enables
        /// a ranged query) and the value will be formatted using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="format">The format that should be used when adding the date to the value set.</param>
        /// <param name="key">The key of the field.</param>
        public static IndexingItemEventArgs IndexDateWithFormat(this IndexingItemEventArgs e, string format, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;
            
            // Try to parse the first value of the field
            if (TryParseDateTime(values.FirstOrDefault(), out DateTime dateTime)) return e;

            // Add a new range field with the date in the specified format
            e.ValueSet.TryAdd($"{key}_range", dateTime.ToString(format));

            return e;

        }

        /// <summary>
        /// Adds additional fields to make the date and time of the field with the specified <paramref name="key"/> more searchable.
        /// </summary>
        /// <param name="e">The event args for the item being indexed.</param>
        /// <param name="key">The key of the field holding the date and time.</param>
        public static IndexingItemEventArgs IndexDateExtended(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;

            // Get the first value of the field
            switch (values.FirstOrDefault()) {

                case DateTime dt:
                    IndexDateTime(e, key, dt);
                    break;

                case string str:
                    if (DateTime.TryParseExact(str, ExamineDateFormats.Umbraco, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt2)) {
                        IndexDateTime(e, key, dt2);
                    }
                    break;

            }

            return e;

        }

        /// <summary>
        /// Adds a textual representation of the block list value from property with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="logger">The logger to be used for logging errors.</param>
        /// <param name="indexingHelper">The indexing helper to be used for getting a textual representation of block list values.</param>
        /// <param name="umbracoContext">An Umbraco context used for looking up the corresponding content item of the current result.</param>
        /// <param name="key">The key (or alias) of the property holding the block list value.</param>
        /// <param name="newKey">If specified, the value of this parameter will be used for the key of the new field added to the valueset.</param>
        /// <param name="newKeySuffix">If specified, and <paramref name="newKey"/> is not also specified, the value of this parameter will be appended to <paramref name="key"/>, and used for the key of the new field added to the valueset.</param>
        public static IndexingItemEventArgs IndexBlockList(this IndexingItemEventArgs e, ILogger logger, IIndexingHelper indexingHelper, IUmbracoContext umbracoContext, string key, string newKey = null, string newKeySuffix = null) {

            // The ID is numeric, but stored as a string, so we need to parse it
            if (!int.TryParse(e.ValueSet.Id, out int id)) return e;

            // Look up the content node in the content cache
            IPublishedContent content = umbracoContext.Content.GetById(id);

            // Call the method overload to handle the rest
            return IndexBlockList(e, logger, indexingHelper, content, key, newKey, newKeySuffix);

        }

        /// <summary>
        /// Adds a textual representation of the block list value from property with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="logger">The logger to be used for logging errors.</param>
        /// <param name="indexingHelper">The indexing helper to be used for getting a textual representation of block list values.</param>
        /// <param name="content">The content item holding the block list value.</param>
        /// <param name="key">The key (or alias) of the property holding the block list value.</param>
        /// <param name="newKey">If specified, the value of this parameter will be used for the key of the new field added to the valueset.</param>
        /// <param name="newKeySuffix">If specified, and <paramref name="newKey"/> is not also specified, the value of this parameter will be appended to <paramref name="key"/>, and used for the key of the new field added to the valueset.</param>
        public static IndexingItemEventArgs IndexBlockList(this IndexingItemEventArgs e, ILogger logger, IIndexingHelper indexingHelper, IPublishedContent content, string key, string newKey = null, string newKeySuffix = null) {

            // Validate the content and the property
            if (content == null || !content.HasProperty(key)) return e;

            // Get the block list
            BlockListModel blockList = content.Value<BlockListModel>(key);
            if (blockList == null) return e;

            // Determine the new key
            newKey ??= $"{key}{newKeySuffix ?? "_search"}";

            // Get the searchable text via the indexing helper
            try {
                string text = indexingHelper.GetSearchableText(blockList);
                if (!string.IsNullOrWhiteSpace(text)) e.ValueSet.TryAdd(newKey, text);
            } catch (Exception ex) {
                logger.LogError(ex, "Failed indexing block list in property {Property} on page with ID {Id}.", key, content.Id);
            }

            return e;

        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden (excluded) from search results.
        /// </summary>
        /// <param name="e"></param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e) {
            return AddHideFromSearch(e, default(HashSet<int>));
        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden
        /// (excluded) from search results.
        ///
        /// The <paramref name="ignoreId"/> parameter can be used to specify an area of the website that should
        /// automatically be hidden from search results. This is done by checking whether the ID of the
        /// <paramref name="ignoreId"/> parameter is part of the <c>path</c> field of the valueset for the current node.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ignoreId">The ID for which the node itself and it's descendants should be hidden.</param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e, int ignoreId) {
            return AddHideFromSearch(e, new HashSet<int> { ignoreId });
        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden
        /// (excluded) from search results.
        ///
        /// The <paramref name="ignoreIds"/> parameter can be used to specify areas of the website that should
        /// automatically be hidden from search results. This is done by checking whether at least one of the IDs the
        /// <paramref name="ignoreIds"/> parameter is part of the <c>path</c> field of the valueset for the current node.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ignoreIds">The IDs for which it self and it's descendants should be hidden.</param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e, params int[] ignoreIds) {
            return AddHideFromSearch(e, new HashSet<int>(ignoreIds));
        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden
        /// (excluded) from search results.
        ///
        /// The <paramref name="ignoreIds"/> parameter can be used to specify areas of the website that should
        /// automatically be hidden from search results. This is done by checking whether at least one of the IDs the
        /// <paramref name="ignoreIds"/> parameter is part of the <c>path</c> field of the valueset for the current node.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ignoreIds">The IDs for which it self and it's descendants should be hidden.</param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e, HashSet<int> ignoreIds) {

            e.ValueSet.Values.TryGetValue(ExamineFields.Path, out List<object> objList);
            int[] ids = StringUtils.ParseInt32Array(objList?.FirstOrDefault()?.ToString());

            if (ignoreIds != null && ids.Any(ignoreIds.Contains)) {
                e.ValueSet.Set(ExamineFields.HideFromSearch, "1");
                return e;
            }

            if (e.ValueSet.Values.ContainsKey(ExamineFields.HideFromSearch)) return e;

            // create empty value
            e.ValueSet.TryAdd(ExamineFields.HideFromSearch, "0");

            return e;

        }

        /// <summary>
        /// Adds new fields with lower cased versions of the <c>nodeName</c>, <c>title</c> and <c>teaser</c> fields.
        /// </summary>
        /// <param name="e"></param>
        public static IndexingItemEventArgs AddDefaultLciFields(this IndexingItemEventArgs e) {
            return AddLciFields(e, _defaultLciFields);
        }

        /// <summary>
        /// Adds a new field with a lower cased version of the specified <paramref name="field"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="field">The key of the field that should have a lower cased version.</param>
        public static IndexingItemEventArgs AddLciField(this IndexingItemEventArgs e, string field) {
            
            if (string.IsNullOrWhiteSpace(field)) throw new ArgumentNullException(nameof(field));

            // Skip non-content types
            if (e.ValueSet.Category != IndexTypes.Content) return e;
            
            // Calculate the LCI key
            string lciKey = $"{field}_lci";

            // Skip if the LCI key already exists
            if (e.ValueSet.Values.ContainsKey(lciKey)) return e;

            // Get each value with "key" and add the lowwer cased versions to a new field
            foreach (object value in e.ValueSet.GetValues(field)) {
                e.ValueSet.Add(lciKey, value.ToString()?.ToLowerInvariant());
            }

            return e;

        }

        /// <summary>
        /// Adds new fields with lower cased versions of the specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="fields">The keys of the fields that should have a lower cased version.</param>
        public static IndexingItemEventArgs AddLciFields(this IndexingItemEventArgs e, params string[] fields) {

            // Skip non-content types
            if (e.ValueSet.Category != IndexTypes.Content) return e;

            foreach (string key in fields) {
                AddLciField(e, key);
            }

            return e;

        }

        /// <summary>
        /// Splits the boost words of the <see cref="ExamineFields.BoostWords"/> field into individual fields.
        /// </summary>
        /// <param name="e">The event args for the item being indexed.</param>
        public static IndexingItemEventArgs AddBoostWords(this IndexingItemEventArgs e) {
            return AddBoostWords(e, ExamineFields.BoostWords);
        }

        /// <summary>
        /// Splits the boost words of the specified <paramref name="field"/> into individual fields.
        /// </summary>
        /// <param name="e">The event args for the item being indexed.</param>
        /// <param name="field">The name of the field holding the boost words.</param>
        public static IndexingItemEventArgs AddBoostWords(this IndexingItemEventArgs e, string field) {

            // Skip non-content types
            if (e.ValueSet.Category != IndexTypes.Content) return e;

            // Attempt to the first string value of the specified field
            if (!TryGetString(e, field, out string rawValue)) return e;

            // Attempt to parse the JSON array
            if (!JsonUtils.TryParseJsonArray(rawValue, out JArray array)) return e;

            // Initialize a new dictionary for keeping track of the boost words and their boosted value
            Dictionary<int, List<string>> temp = new();

            // Iterate through the array
            foreach (JToken token in array) {

                // Should always be JObject, but doesn't hurt to check
                if (token is not JObject obj) continue;

                // Extract the values from the JSON
                int boost = obj.GetInt32("boost");
                string value = obj.GetString("value")?.Trim().ToLowerInvariant();
                if (boost <= 0 || string.IsNullOrWhiteSpace(value)) continue;

                // Add the individual words to the dictionary by their boost value
                if (!temp.TryGetValue(boost, out List<string> list)) temp.Add(boost, list = new List<string>());
                list.Add(value);

            }

            // Add new fields for each boost value and their respective words
            foreach ((int boost, List<string> list) in temp) {
                e.ValueSet.TryAdd($"{ExamineFields.BoostWords}_{boost}", string.Join(" ", list));
            }

            return e;

        }

        private static bool TryParseDateTime(object value, out DateTime result) {

            switch (value) {

                case DateTime dt:
                    result = dt;
                    return true;

                case string str:
                    return DateTime.TryParseExact(str, ExamineDateFormats.Umbraco, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out result);

                default:
                    result = default;
                    return false;

            }

        }

        private static void IndexDateTime(IndexingItemEventArgs e, string key, DateTime dateTime) {
            e.ValueSet.TryAdd($"{key}_search", dateTime.ToString(ExamineDateFormats.Sortable));
            e.ValueSet.TryAdd($"{key}_ticks", dateTime.Ticks.ToString());
            e.ValueSet.TryAdd($"{key}_year", dateTime.Year);
            e.ValueSet.TryAdd($"{key}_month", dateTime.Month);
            e.ValueSet.TryAdd($"{key}_day", dateTime.Day);
            e.ValueSet.TryAdd($"{key}_week", Iso8601Utils.GetWeekNumber(dateTime));
        }

    }

}