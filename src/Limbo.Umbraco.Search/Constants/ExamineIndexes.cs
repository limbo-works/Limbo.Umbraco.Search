using System;

namespace Limbo.Umbraco.Search.Constants;

/// <summary>
/// Class with various constants for default and common index names.
/// </summary>
public static class ExamineIndexes {

    /// <summary>
    /// Gets the name of the external index.
    /// </summary>
    public const string ExternalIndex = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.ExternalIndexName;

    /// <summary>
    /// Gets the name of the internal index.
    /// </summary>
    public const string InternalIndex = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.InternalIndexName;

    /// <summary>
    /// Gets the name of the members index.
    /// </summary>
    public const string MembersIndex = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.MembersIndexName;

    /// <summary>
    /// Gets the name of the index created by the <strong>UmbracoCms.UmbracoExamine.PDF</strong> package.
    /// </summary>
    /// <see>
    ///     <cref>https://www.nuget.org/packages/UmbracoCms.UmbracoExamine.PDF/</cref>
    /// </see>
    public const string PdfIndex = "PDFIndex";

    /// <summary>
    /// Gets the name of the external index.
    /// </summary>
    [Obsolete("Use the 'ExternalIndex' constant instead.")]
    public const string ExternalIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.ExternalIndexName;

    /// <summary>
    /// Gets the name of the internal index.
    /// </summary>
    [Obsolete("Use the 'InternalIndex' constant instead.")]
    public const string InternalIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.InternalIndexName;

    /// <summary>
    /// Gets the name of the members index.
    /// </summary>
    [Obsolete("Use the 'MembersIndex' constant instead.")]
    public const string MembersIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.MembersIndexName;

    /// <summary>
    /// Gets the name of the index created by the <strong>UmbracoCms.UmbracoExamine.PDF</strong> package.
    /// </summary>
    /// <see>
    ///     <cref>https://www.nuget.org/packages/UmbracoCms.UmbracoExamine.PDF/</cref>
    /// </see>
    [Obsolete("Use the 'PdfIndex' constant instead.")]
    public const string PdfIndexName = "PDFIndex";

}