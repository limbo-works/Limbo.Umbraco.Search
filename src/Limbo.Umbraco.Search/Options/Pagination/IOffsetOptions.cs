namespace Limbo.Umbraco.Search.Options.Pagination {

    /// <summary>
    /// Interface describing a paginated search based on <see cref="Limit"/> and <see cref="Offset"/>.
    /// </summary>
    public interface IOffsetOptions : ISearchOptions {

        /// <summary>
        /// Get or sets the offset.
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// Get or sets the offset.
        /// </summary>
        int Limit { get; set; }

    }

}