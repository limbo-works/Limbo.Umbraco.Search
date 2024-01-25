namespace Limbo.Umbraco.Search.Constants;

/// <summary>
/// Class with various constants for default and common index names.
/// </summary>
public static class ExamineIndexes {

    /// <summary>
    /// Gets the name of the external index.
    /// </summary>
    public const string ExternalIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.ExternalIndexName;

    /// <summary>
    /// Gets the name of the internal index.
    /// </summary>
    public const string InternalIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.InternalIndexName;

    /// <summary>
    /// Gets the name of the members index.
    /// </summary>
    public const string MembersIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.MembersIndexName;

    /// <summary>
    /// Gets the name of the index created by the <strong>UmbracoCms.UmbracoExamine.PDF</strong> package.
    /// </summary>
    /// <see>
    ///     <cref>https://www.nuget.org/packages/UmbracoCms.UmbracoExamine.PDF/</cref>
    /// </see>
    public const string PdfIndexName = "PDFIndex";

}