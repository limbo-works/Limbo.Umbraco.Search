﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Limbo.Umbraco.Search.Models.Groups;

/// <summary>
/// Class representing a grouped search results
/// </summary>
public class GroupedSearchResult {

    /// <summary>
    /// Gets the individual groups making up the overall search result.
    /// </summary>
    [JsonProperty("groups")]
    public IReadOnlyList<SearchGroupResultList> Groups { get; }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="groups"/>.
    /// </summary>
    /// <param name="groups">The groups making up the overall search result.</param>
    public GroupedSearchResult(IEnumerable<SearchGroupResultList> groups) {
        Groups = groups.ToArray();
    }

}