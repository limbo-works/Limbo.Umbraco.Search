﻿using Limbo.Umbraco.Search.Options.Pagination;

namespace Limbo.Umbraco.Search.Options {

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
        public ContentTypeList ContentTypes { get; set; }

        #endregion

        #region Constructors

        public OffsetSearchOptionsBase() {
            ContentTypes = new ContentTypeList();
        }

        #endregion

        #region Member methods

        protected override void SearchType(ISearchHelper helper, QueryList query) {
            if (ContentTypes == null || ContentTypes.Count == 0) return;
            query.AppendNodeTypeAliases(ContentTypes);
        }

        #endregion

    }

}