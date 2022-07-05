namespace Limbo.Umbraco.Search.Constants {

    /// <summary>
    /// Class with constants for typical lower case fields in Examine.
    /// </summary>
    public static class ExamineLowerCaseFields {

        internal const string Suffix = "_lci";

        /// <summary>
        /// Gets the key of the lower case field with the name of the node.
        /// </summary>
        public const string NodeName = ExamineFields.NodeName + Suffix;

        /// <summary>
        /// Gets the key of the lower case field with the teaser of the node.
        /// </summary>
        public const string Teaser = ExamineFields.Teaser + Suffix;

        /// <summary>
        /// Gets the key of the lower case field with the title of the node.
        /// </summary>
        public const string Title = ExamineFields.Title + Suffix;

    }

}