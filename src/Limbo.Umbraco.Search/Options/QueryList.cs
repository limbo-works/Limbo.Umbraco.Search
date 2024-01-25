﻿using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Umbraco.Search.Options;

/// <summary>
/// Class representing a list of queries.
/// </summary>
public class QueryList {

    /// <summary>
    /// Internal list for building up the query list.
    /// </summary>
    protected readonly List<object> List;

    /// <summary>
    /// Gets the type of the list.
    /// </summary>
    public QueryListType Type { get; }

    /// <summary>
    /// Gets whether this list defines queries that should be excluded (opposite of included, which is default).
    /// </summary>
    public bool Exclude { get; }

    /// <summary>
    /// Initializes a new <see cref="QueryListType.And"/> based query list.
    /// </summary>
    public QueryList() : this(QueryListType.And) { }

    /// <summary>
    /// Initializes a new query list based on the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type of the query list.</param>
    public QueryList(QueryListType type) {
        Type = type;
        List = new List<object>();
    }

    /// <summary>
    /// Initializes a new query list based on the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type of the query list.</param>
    /// <param name="exclude">Whether this list defines queries that should be excluded.</param>
    public QueryList(QueryListType type, bool exclude) {
        Type = type;
        Exclude = exclude;
        List = new List<object>();
    }

    /// <summary>
    /// Returns a raw query based on this query list.
    /// </summary>
    /// <returns>A string representing the raw query.</returns>
    public virtual string GetRawQuery() {
        return $"{(Exclude ? "-" : "")}({string.Join($" {Type.ToUpper()} ", from item in List select item.ToString())})";
    }

    /// <inheritdoc />
    public override string ToString() {
        return GetRawQuery();
    }

    /// <summary>
    /// Adds a new sub query to the query list.
    /// </summary>
    /// <param name="value">The sub query. May be either a <see cref="string"/> or another <see cref="QueryList"/>.</param>
    public virtual void Add(object value) {

        switch (value) {

            case null:
                return;

            case string str:
                List.Add(str);
                break;

            case QueryList list:
                List.Add(list);
                break;

            default:
                throw new Exception($"Unsupported type: {value.GetType()}");

        }

    }

}