using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Limbo.Umbraco.Search.Options {

    /// <summary>
    /// Class representing a list of content types to search.
    /// </summary>
    public class ContentTypeList : IEnumerable<string> {

        private readonly List<string> _contentTypes;

        #region Properties

        /// <summary>
        /// Gets the amount of content types added to the list.
        /// </summary>
        public int Count => _contentTypes.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new, empty content type list.
        /// </summary>
        public ContentTypeList() {
            _contentTypes = new List<string>();
        }

        /// <summary>
        /// Initializes a new content type list based on the specified <paramref name="contentTypes"/>.
        /// </summary>
        /// <param name="contentTypes">The content types that should initially make up the list.</param>
        public ContentTypeList(IEnumerable<string> contentTypes) {
            _contentTypes = contentTypes.ToList();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Adds a new content type with the specified <paramref name="contentTypeAlias"/>.
        /// </summary>
        /// <param name="contentTypeAlias">The alias of the content type to be added.</param>
        public void Add(string contentTypeAlias) {
            _contentTypes.Add(contentTypeAlias);
        }

        /// <summary>
        /// Adds the content types with the specified <paramref name="contentTypeAliases"/>.
        /// </summary>
        /// <param name="contentTypeAliases">The aliases of the content types to be added.</param>
        public void AddRange(params string[] contentTypeAliases) {
            _contentTypes.AddRange(contentTypeAliases);
        }
        
        /// <summary>
        /// Adds the content types with the specified <paramref name="contentTypeAliases"/>.
        /// </summary>
        /// <param name="contentTypeAliases">The aliases of the content types to be added.</param>
        public void AddRange(IEnumerable<string> contentTypeAliases) {
            _contentTypes.AddRange(contentTypeAliases);
        }

        /// <inheritdoc />
        public IEnumerator<string> GetEnumerator() {
            return _contentTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }

}