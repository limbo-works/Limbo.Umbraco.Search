using Limbo.Umbraco.Search.Options.Pagination;

namespace Limbo.Umbraco.Search.Options {

    /// <summary>
    /// Class implementing the <see cref="IOffsetOptions"/> interface.
    /// </summary>
    public class OffsetSearchOptionsBase : SearchOptionsBase, IOffsetOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the offset to be used for pagination.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the limit to be used for pagination.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets a list of content types to be searched.
        /// </summary>
        public ContentTypeList? ContentTypes { get; set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public OffsetSearchOptionsBase() {
            ContentTypes = new ContentTypeList();
        }

        #endregion

        #region Member methods
        
        /// <summary>
        /// Virtual method for limiting the search to specific content types.
        /// </summary>
        /// <param name="searchHelper">A reference to the current <see cref="ISearchHelper"/>.</param>
        /// <param name="query">The query.</param>
        protected override void SearchType(ISearchHelper searchHelper, QueryList query) {
            if (ContentTypes == null || ContentTypes.Count == 0) return;
            query.AppendNodeTypeAliases(ContentTypes);
        }

        #endregion

    }

}