namespace Limbo.Umbraco.Search.Constants {

    /// <summary>
    /// Class with various constants related to Examine.
    /// </summary>
    public static class ExamineConstants {

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

        /// <summary>
        /// Class with constants for typical fields in Examine.
        /// </summary>
        public static class Fields {

            /// <summary>
            /// Gets the key of the field indicating the index type.
            /// </summary>
            public const string IndexType = "__IndexType";
            
            /// <summary>
            /// Gets the key of the field with the key of the node.
            /// </summary>
            public const string Key = "__Key";
            
            /// <summary>
            /// Gets the key of the field with the numeric ID of the node.
            /// </summary>
            public const string NodeId = "__NodeId";
            
            /// <summary>
            /// Gets the key of the field indicating the node type alias of the node.
            /// </summary>
            public const string NodeTypeAlias = "__NodeTypeAlias";
            
            /// <summary>
            /// Gets the key of the field indicating the node's path.
            /// </summary>
            public const string Path = "path";
            
            /// <summary>
            /// Gets the key of the field holding a search-friendly version of the node's path.
            /// </summary>
            public const string PathSearch = "path_search";

            /// <summary>
            /// Gets the key of the field indicating whether the node should be included in search results.
            /// </summary>
            public const string HideFromSearch = "hideFromSearch";

        }

    }

}