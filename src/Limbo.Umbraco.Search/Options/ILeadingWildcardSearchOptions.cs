namespace Limbo.Umbraco.Search.Options;

/// <summary>
/// Interface controlling whether leading wildcard should be allowed.
/// </summary>
public interface ILeadingWildcardSearchOptions : ISearchOptions {

    /// <summary>
    /// Gets or sets whether leading wildcard should be allowed.
    /// </summary>
    bool? AllowLeadingWildcard { get; set; }

}