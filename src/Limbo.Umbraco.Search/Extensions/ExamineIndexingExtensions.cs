using System;
using Examine;
using Limbo.Umbraco.Search.Indexing;
using Microsoft.Extensions.Logging;
using Skybrud.Essentials.Umbraco.Examine;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Search.Extensions;

/// <summary>
/// Static class with various extension methods for <see cref="ExamineIndexingExtensions"/>.
/// </summary>
public static class ExamineIndexingExtensions {

    /// <summary>
    /// Adds a textual representation of the block list value from property with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="logger">The logger to be used for logging errors.</param>
    /// <param name="indexingHelper">The indexing helper to be used for getting a textual representation of block list values.</param>
    /// <param name="umbracoContext">An Umbraco context used for looking up the corresponding content item of the current result.</param>
    /// <param name="key">The key (or alias) of the property holding the block list value.</param>
    /// <param name="newKey">If specified, the value of this parameter will be used for the key of the new field added to the value set.</param>
    /// <param name="newKeySuffix">If specified, and <paramref name="newKey"/> is not also specified, the value of this parameter will be appended to <paramref name="key"/>, and used for the key of the new field added to the valueset.</param>
    public static IndexingItemEventArgs IndexBlockList(this IndexingItemEventArgs e, ILogger logger, IIndexingHelper indexingHelper, IUmbracoContext umbracoContext, string key, string? newKey = null, string? newKeySuffix = null) {

        // The ID is numeric, but stored as a string, so we need to parse it
        if (!int.TryParse(e.ValueSet.Id, out int id)) return e;

        // Look up the content node in the content cache
        IPublishedContent? content = umbracoContext.Content.GetById(id);

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
    /// <param name="newKey">If specified, the value of this parameter will be used for the key of the new field added to the value set.</param>
    /// <param name="newKeySuffix">If specified, and <paramref name="newKey"/> is not also specified, the value of this parameter will be appended to <paramref name="key"/>, and used for the key of the new field added to the value set.</param>
    public static IndexingItemEventArgs IndexBlockList(this IndexingItemEventArgs e, ILogger logger, IIndexingHelper indexingHelper, IPublishedContent? content, string key, string? newKey = null, string? newKeySuffix = null) {

        // Validate the content and the property
        if (content == null || !content.HasProperty(key)) return e;

        // Get the block list
        BlockListModel? blockList = content.Value<BlockListModel>(key);
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

}